# Employee Information System

A production-quality portfolio Employee Information System built with **ASP.NET Core 8, C#, Entity Framework Core, SQL Server, Clean Architecture, CQRS/MediatR, JWT authentication, Docker, GitHub Actions, and Azure**.

The project demonstrates enterprise software development practices including clean architecture, separation of concerns, repository-based data access, validation, authentication and authorization, automated testing, containerization, CI/CD, audit/history tracking, and secure file management.

> ✅ **Status:** v1.0.0 — Production-Ready Portfolio Milestone

---

# Current Version

**v1.0.0**

The project was developed incrementally from **v0.1.0 through v1.0.0**, with each milestone representing a major development stage.

### v1.0.0 Highlights

- Clean Architecture
- CQRS with MediatR
- Repository Pattern
- Dependency Injection
- SOLID principles
- ASP.NET Core 8 Web API
- Entity Framework Core Code First
- SQL Server / Azure SQL
- JWT Bearer Authentication
- Role and Permission-Based Authorization
- Dynamic Permission Policies
- FluentValidation
- Global Exception Handling
- ProblemDetails
- Swagger / OpenAPI
- Health Checks
- Audit and History Tracking
- Soft Delete
- Pagination and Search
- Employee File Upload / Download
- Docker and Docker Compose
- GitHub Actions CI/CD
- GitHub Container Registry
- Azure App Service
- Azure SQL
- 57 Automated Tests

---

# Technology Stack

## Backend

- C#
- .NET 8
- ASP.NET Core 8 Web API
- Entity Framework Core 8
- SQL Server
- Azure SQL

## Architecture & Design

- Clean Architecture
- Layered Architecture
- CQRS
- MediatR
- Repository Pattern
- Dependency Injection
- Dependency Inversion
- SOLID Principles
- Separation of Concerns

## Security

- JWT Bearer Authentication
- Claims-Based Authentication
- Role-Based Authorization
- Permission-Based Authorization
- Dynamic Permission Policies
- Password Hashing
- Authentication / Authorization Middleware

## Validation & API

- FluentValidation
- MediatR Pipeline Behaviors
- Global Exception Handling
- ProblemDetails
- Health Checks
- Swagger / OpenAPI

## Database

- Entity Framework Core Code First
- EF Core Migrations
- SQL Server
- Azure SQL
- Database Seeding
- Audit / History Tracking
- Soft Delete
- Pagination
- Search
- SQL Connection Resiliency / Retry

## Testing

- xUnit
- Unit Testing
- Validation Testing
- Authentication / Authorization Testing
- Exception Handling Testing
- CRUD Testing
- File Upload / Download Testing

**57 automated tests passing**

## DevOps & Cloud

- Git
- GitHub
- GitHub Actions
- GitHub Container Registry (GHCR)
- Docker
- Docker Compose
- Azure App Service
- Azure SQL
- CI/CD

## Frontend

React and Bootstrap are planned for **Phase 2**.

---

# Features

## Employee Management

- Create Employee
- Update Employee
- View Employees
- Search Employees
- Pagination
- Soft Delete / Deactivation
- Employee Audit History

## User Management

- User Authentication
- User Account Management
- Password Hashing
- Password Change Requirement
- Account Status Management
- Authentication / Authorization

## Role & Permission Management

- Role Management
- Function / Permission Management
- Role-Function Assignment
- Employee-Role Assignment
- Dynamic Permission Policies
- Permission-Based API Authorization

## Organization Management

- Department Management
- Position Management
- Department Audit History
- Position Audit History

## File Management

- Employee Document Management
- File Upload
- File Download
- File History
- Permission-Controlled File Access

## API & Infrastructure

- RESTful Web API
- Swagger / OpenAPI
- JWT Bearer Security
- Global Exception Handling
- Standardized ProblemDetails Responses
- Health Checks
- Database Migration on Startup
- Idempotent Database Seeding

---

# Automated Testing

The project includes **57 automated xUnit tests** covering:

- Employee CRUD
- User CRUD
- Department CRUD
- Position CRUD
- Role CRUD
- Function / Permission CRUD
- RoleFunction CRUD
- EmployeeRole CRUD
- File Upload
- File Retrieval
- File Download
- Validation
- Authentication
- Authorization
- Exception Handling

Current result:

```text
Total:     57
Passed:    57
Failed:     0
Skipped:    0
```

---

# CI/CD

GitHub Actions automatically performs the following on pushes to `main` and pull requests:

1. Restore .NET dependencies
2. Build the solution
3. Run automated tests
4. Build the Docker image
5. Publish the Docker image to GitHub Container Registry
6. Deploy the latest image to Azure App Service

The application is containerized using Docker and deployed to Azure App Service.

---

# Cloud Deployment

The application has been deployed to:

- **Azure App Service**
- **Azure SQL**
- **GitHub Container Registry**

The deployment includes:

- Application configuration
- Azure SQL connectivity
- Database migration
- Database seeding
- Health monitoring
- Docker container deployment
- GitHub Actions CI/CD
- End-to-end deployment verification

---

# Project Structure

```text
EmployeeInformationSystem
│
├── src
│   ├── EmployeeInformationSystem.API
│   ├── EmployeeInformationSystem.Application
│   ├── EmployeeInformationSystem.Domain
│   ├── EmployeeInformationSystem.Infrastructure
│   └── EmployeeInformationSystem.Persistence
│
├── tests
│   └── EmployeeInformationSystem.Tests
│
├── .github
│   └── workflows
│       └── ci.yml
│
├── Dockerfile
├── docker-compose.yml
├── README.md
├── ARCHITECTURE.md
└── FOLDER_STRUCTURE.md
```

---

# Development Roadmap

| Version | Description | Status |
|---|---|---|
| v0.1.0 | Domain Model | ✅ Completed |
| v0.2.0 | Persistence Layer | ✅ Completed |
| v0.3.0 | Repository Pattern | ✅ Completed |
| v0.4.0 | CQRS + MediatR | ✅ Completed |
| v0.5.0 | Authentication | ✅ Completed |
| v0.6.0 | Employee CRUD | ✅ Completed |
| v0.7.0 | Role & Permission Authorization | ✅ Completed |
| v0.8.0 | File Upload & Document Management | ✅ Completed |
| v0.9.0 | Docker & GitHub Actions | ✅ Completed |
| v1.0.0 | Production-Ready Portfolio Milestone | ✅ Completed |

---

# Phase 2 — Planned

The next development phase is planned to extend the system with a modern frontend and additional production capabilities.

Planned items include:

- React frontend
- Bootstrap 5 UI
- API integration
- Frontend authentication
- Improved user experience
- Additional cloud/storage enhancements

---

# Project Goals

This project demonstrates practical experience with:

- Enterprise software architecture
- Modern .NET development
- Clean code and SOLID principles
- RESTful API development
- Authentication and authorization
- Database design and persistence
- Automated testing
- Containerization
- CI/CD
- Cloud deployment
- Production-oriented development practices
- Maintainable and scalable application design

---

# Documentation

Additional documentation:

- `ARCHITECTURE.md`
- `FOLDER_STRUCTURE.md`

---

# Author

**Joshua Sabat**

Senior Software Engineer

Singapore

Open to Senior Software Engineer, Senior .NET Developer, and hands-on Technical Lead opportunities.

---

# Repository

GitHub:

https://github.com/sabatjoshua/employee-information-system
