<h1 align="center">🚪 MedGuide API Gateway</h1>

<p align="center">
  <b>ASP.NET Core | Ocelot API Gateway | Microservices Architecture</b>
</p>

<p align="center">
  A centralized API Gateway that serves as the single entry point for all MedGuide Healthcare Platform microservices, providing unified routing, security, and service orchestration.
</p>

---

## 🏗️ Project Overview

The MedGuide API Gateway acts as the communication hub between clients and backend microservices.

Instead of calling individual services directly, all requests pass through the API Gateway, which routes them to the appropriate service.

> ⚠️ This repository contains only the API Gateway layer. To run the complete MedGuide Healthcare Platform, all dependent microservices must be running and properly configured.

---

## 🔗 Required Microservices

The API Gateway depends on the following microservices to function properly.

| Service | Repository |
|----------|------------|
| 🔐 Identity & Authentication Service | [Identity-Auth-MicroService](https://github.com/MohamedTarek69/Identity-Auth-MicroService) |
| 🧑‍⚕️ Patient Management Service | [PatiantMicroService](https://github.com/MohamedTarek69/PatiantMicroService) |
| 🏥 Doctor & Clinic Management Service | [DoctorClinicMicroServices](https://github.com/MohamedTarek69/DoctorClinicMicroServices) |
| 🤖 AI ChatBot Service | [AIChatBotMicroService](https://github.com/MohamedTarek69/AIChatBotMicroService) |

### Service Startup Order

```text
1. Identity Service
2. Patient Service
3. Doctor & Clinic Service
4. AI ChatBot Service
5. API Gateway
```

### MedGuide Ecosystem Repositories

- 🔐 Identity Service  
  https://github.com/MohamedTarek69/Identity-Auth-MicroService

- 🧑‍⚕️ Patient Service  
  https://github.com/MohamedTarek69/PatiantMicroService

- 🏥 Doctor & Clinic Service  
  https://github.com/MohamedTarek69/DoctorClinicMicroServices

- 🤖 AI ChatBot Service  
  https://github.com/MohamedTarek69/AIChatBotMicroService

---

## 🎯 Goals

- Provide a single entry point for all services.
- Simplify client communication.
- Centralize routing and request forwarding.
- Improve maintainability and scalability.
- Enable future security and monitoring enhancements.

---

## 🌐 Microservices Ecosystem

The API Gateway integrates the following services:

| Service | Description |
|----------|-------------|
| 🔐 Identity Service | Authentication, Authorization, JWT & User Management |
| 🧑‍⚕️ Patient Service | Patient Profiles, Medical Records & Allergies |
| 🏥 Doctor & Clinic Service | Doctors, Clinics, Appointments & Time Slots |
| 🤖 AI ChatBot Service | AI-Powered Symptom Analysis & Medical Guidance |

---

## 🧱 System Architecture

```text
                     ┌─────────────────┐
                     │     Frontend    │
                     └────────┬────────┘
                              │
                              ▼
                  ┌───────────────────────┐
                  │    API Gateway        │
                  │      (Ocelot)         │
                  └─────────┬─────────────┘
                            │
       ┌────────────────────┼────────────────────┐
       │                    │                    │
       ▼                    ▼                    ▼

┌─────────────┐    ┌─────────────┐    ┌─────────────────┐
│ Identity    │    │ Patient     │    │ Doctor & Clinic │
│ Service     │    │ Service     │    │ Service         │
└─────────────┘    └─────────────┘    └─────────────────┘
       │                    │
       └────────────┬───────┘
                    ▼
         ┌───────────────────┐
         │ AI ChatBot Service│
         └───────────────────┘
```

---

## ✨ Main Features

| Feature | Description |
|----------|-------------|
| 🚪 Single Entry Point | Unified access to all services |
| 🔄 Request Routing | Routes requests to target services |
| 🔗 Service Aggregation | Centralized communication |
| 📈 Scalability | Supports independent service scaling |
| 🧩 Loose Coupling | Decouples clients from services |
| ⚙️ Centralized Configuration | Service routing through Ocelot |

---

## 🧰 Tech Stack

| Category | Technology |
|-----------|-------------|
| Gateway | Ocelot API Gateway |
| Framework | ASP.NET Core |
| Architecture | Microservices |
| Authentication | JWT Bearer |
| Documentation | Swagger |
| Configuration | Ocelot JSON Configuration |
| Version Control | Git & GitHub |

---

## 🔀 Routing Responsibilities

### Authentication Requests

```text
auth/Clinic/Authentication/*
        │
        ▼
Identity Service
```

Examples:

- Login
- Register
- Refresh Token
- Logout
- User Management

---

### Patient Requests

```text
/patient/Patiant/*
        │
        ▼
Patient Service
```

Examples:

- Patient Profiles
- Medical Records
- Allergies
- Patient Details

---

### Doctor & Clinic Requests

```text
/api/doctor/*
/api/doctorclinics/*
/api/appointments/*
/api/timeslots/*
        │
        ▼
Doctor & Clinic Service
```

Examples:

- Doctor Management
- Clinic Management
- Appointment Booking
- Time Slot Scheduling

---

### AI Requests

```text
ai/api/chatbot/*
        │
        ▼
AI ChatBot Service
```

Examples:

- Symptom Analysis
- AI Medical Guidance
- Chat History

---

## 🔒 Security

### JWT Authentication

The API Gateway validates and forwards JWT-protected requests to downstream services.

Benefits:

- Centralized Security
- Secure Service Communication
- Consistent Authentication Flow

---

## 🚀 Benefits of API Gateway

### For Clients

- Single API Endpoint
- Simplified Integration
- Consistent API Experience

### For Developers

- Easier Service Management
- Better Scalability
- Independent Deployments
- Centralized Routing

### For Operations

- Monitoring
- Logging
- Future Rate Limiting
- Future Load Balancing

---

## 📦 Future Enhancements

- 🐳 Docker Deployment
- ☸️ Kubernetes Support
- 📊 Centralized Monitoring
- 📈 Load Balancing
- 🚦 Rate Limiting
- 📝 Request Logging
- 🔍 Distributed Tracing
- 🔔 Notification Service Integration

---

## 🚀 Getting Started

### Clone Repository

```bash
git clone https://github.com/MohamedTarek69/ApiGateway.git
```

### Configure Ocelot

Update:

```json
ocelot.json
```

### Run Gateway

```bash
dotnet run
```

### Access Gateway

```text
http://localhost:<gateway-port>
```

---

## 👨‍💻 Author

**Mohamed Tarek**

- GitHub: https://github.com/MohamedTarek69

---

<p align="center">
🚪 Single Entry Point for the MedGuide Healthcare Microservices Ecosystem
</p>
