using ClinicMicroService.ApiGateway.Middlewares;
using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Transforms;

namespace ClinicMicroService.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services
                   .AddReverseProxy()
                   .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
            //.AddTransforms(transformBuilderContext =>
            //{
            //    transformBuilderContext.AddResponseTransform(async transformContext =>
            //    {
            //        var proxyResponse = transformContext.ProxyResponse;
            //        if (proxyResponse == null)
            //            return;

            //        var statusCode = (int)proxyResponse.StatusCode;

            //        // Only handle errors
            //        if (statusCode < 400)
            //            return;

            //        var httpContext = transformContext.HttpContext;

            //        var originalBody = proxyResponse.Content == null
            //            ? null
            //            : await proxyResponse.Content.ReadAsStringAsync();

            //        httpContext.Response.Headers.Remove("Content-Length");
            //        httpContext.Response.ContentType = "application/json";

            //        string type;
            //        string title;

            //        switch (statusCode)
            //        {
            //            case StatusCodes.Status400BadRequest:
            //                type = "BadRequest";
            //                title = "Gateway.BadRequest";
            //                break;

            //            case StatusCodes.Status401Unauthorized:
            //                type = "Unauthorized";
            //                title = "Gateway.Unauthorized";
            //                break;

            //            case StatusCodes.Status403Forbidden:
            //                type = "Forbidden";
            //                title = "Gateway.Forbidden";
            //                break;

            //            case StatusCodes.Status404NotFound:
            //                type = "NotFound";
            //                title = "Gateway.NotFound";
            //                break;

            //            case StatusCodes.Status409Conflict:
            //                type = "Conflict";
            //                title = "Gateway.Conflict";
            //                break;

            //            case StatusCodes.Status500InternalServerError:
            //                type = "InternalServerError";
            //                title = "Gateway.InternalError";
            //                break;

            //            default:
            //                if (statusCode >= 500)
            //                {
            //                    type = "DownstreamServerError";
            //                    title = "Gateway.DownstreamServerError";
            //                }
            //                else
            //                {
            //                    type = "DownstreamClientError";
            //                    title = "Gateway.DownstreamClientError";
            //                }
            //                break;
            //        }

            //        var wrappedError = new
            //        {
            //            type = type,
            //            title = title,
            //            status = statusCode,
            //            service = httpContext.Request.Path.StartsWithSegments("/auth") ? "Identity" :
            //                      service = httpContext.Request.Path.StartsWithSegments("/auth") ? "Identity" :
            //                      httpContext.Request.Path.StartsWithSegments("/api") ? "Clinic" :
            //                      httpContext.Request.Path.StartsWithSegments("/patient") ? "Patient" :
            //                      httpContext.Request.Path.StartsWithSegments("/ai") ? "AI" :
            //                      "Unknown",
            //            detail = string.IsNullOrWhiteSpace(originalBody)
            //                ? "The downstream service returned an error."
            //                : originalBody,
            //            traceId = httpContext.TraceIdentifier
            //        };

            //        var json = System.Text.Json.JsonSerializer.Serialize(wrappedError);

            //        transformContext.SuppressResponseBody = true;

            //        await httpContext.Response.WriteAsync(json);
            //    });
            //});

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(7244);
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Global Exception


            // CORS
            app.UseCors("AllowAll");

            // ✅ Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "";

                options.SwaggerEndpoint("/auth/swagger/v1/swagger.json", "Identity Service");
                options.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Clinic Service");
                options.SwaggerEndpoint("/patient/swagger/v1/swagger.json", "Patient Service");

                // AI Service
                options.SwaggerEndpoint("/ai/swagger/v1/swagger.json", "AI Service");
            });

            // Handle Status Codes
            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;

                if (response.HasStarted)
                    return;

                response.ContentType = "application/json";

                var result = new
                {
                    type = "GatewayStatusCodeError",
                    title = "Gateway.HttpError",
                    status = response.StatusCode,
                    detail = $"Gateway returned status code {response.StatusCode}.",
                    traceId = context.HttpContext.TraceIdentifier
                };

                var json = System.Text.Json.JsonSerializer.Serialize(result);
                await response.WriteAsync(json);
            });

            app.MapGet("/", () => Results.Ok("API Gateway is running"));
            app.MapGet("/test", () => Results.Ok("Gateway works"));

            app.MapReverseProxy(proxyPipeline =>
            {
                proxyPipeline.Use(async (context, next) =>
                {
                    await next();

                    if (context.Response.HasStarted)
                        return;

                    var errorFeature = context.Features.Get<IForwarderErrorFeature>();
                    if (errorFeature is not null)
                    {
                        context.Response.StatusCode = StatusCodes.Status502BadGateway;
                        context.Response.ContentType = "application/json";

                        var response = new
                        {
                            type = "BadGateway",
                            title = "Gateway.DownstreamUnavailable",
                            status = 502,
                            detail = "The downstream service is unavailable or unreachable.",
                            traceId = context.TraceIdentifier
                        };

                        var json = System.Text.Json.JsonSerializer.Serialize(response);
                        await context.Response.WriteAsync(json);
                    }
                });
            });

            app.Run();
        }
    }
}