# Employee Information System

A production-quality portfolio Employee Information System built with **ASP.NET Core 8, C#, Entity Framework Core, SQL Server, Clean Architecture, CQRS/MediatR, JWT authentication, React, Bootstrap, Docker, GitHub Actions, and Azure**.

The project demonstrates enterprise software development practices including clean architecture, separation of concerns, repository-based data access, validation, authentication and authorization, automated testing, containerization, CI/CD, audit/history tracking, soft delete, and frontend API integration.

> ✅ **Status:** v1.0.0 — Production-Ready Backend + React Frontend Portfolio Milestone

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
- React
- Vite
- React Router
- Bootstrap 5
- Protected Routes
- REST API Integration
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

## Frontend

- React 19
- Vite
- React Router
- Bootstrap 5
- JWT Authentication
- Protected Routes
- REST API Integration
- Search and Pagination
- Responsive UI
- Configurable API Base URL

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

---

# Features

## Employee Management

- Create Employee
- Update Employee
- View Employee Details
- List Employees
- Search Employees
- Server-side Pagination
- Soft Delete / Deactivation
- Employee Audit History
- Department Selection
- Position Selection
- Department-based Position Filtering
- Gender Selection

## Department Management

- List Departments
- Create Department
- Update Department
- Soft Delete / Deactivation
- Department Audit History
- Permission-Controlled API Access

## Position Management

- List Positions
- Create Position
- Update Position
- Soft Delete / Deactivation
- Position Audit History
- Department-based Position Filtering
- Permission-Controlled API Access

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

# Frontend Application

The React frontend provides a modern web interface for the implemented Employee, Department, and Position management features.

### Authentication

- Login
- JWT token storage
- Protected routes
- Logout

### Employee UI

- Employee list
- Employee search
- Employee pagination
- Employee details
- Create employee
- Edit employee
- Delete employee
- Loading states
- Error handling

### Organization UI

- Department list
- Create department
- Edit department
- Delete department
- Position list
- Create position
- Edit position
- Delete position
- Department-based position filtering

### UI & Configuration

- Bootstrap 5 responsive layout
- Loading indicators
- Error messages
- Consistent navigation
- Configurable API base URL using Vite environment variables
- Production build verification

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

The React application production build has also been successfully verified using:

```bash
npm run build
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
├── frontend
│   └── EmployeeInformationSystem.React
│       ├── src
│       │   ├── components
│       │   ├── pages
│       │   └── services
│       ├── public
│       ├── package.json
│       └── vite.config.js
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

| Version / Phase | Description | Status |
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
| v1.0.0 | Production-Ready Backend Milestone | ✅ Completed |
| Phase 2 | React Frontend + Employee Management UI | ✅ Completed |
| Phase 3 | Department + Position Management UI | ✅ Completed |
| Phase 4 | Additional Enterprise Modules | 📋 Planned |

---

# Phase 2 — Completed

Phase 2 introduced the React frontend and completed the Employee Management user interface.

### Completed

- React frontend
- Vite
- Bootstrap 5
- JWT authentication integration
- Protected routes
- Employee list
- Employee search
- Employee pagination
- Employee details
- Employee create
- Employee edit
- Employee delete
- Department dropdown
- Position filtering by Department
- Gender dropdown
- Loading indicators
- Error handling
- Responsive UI
- API base URL configuration
- React production build verification

---

# Phase 3 — Completed

Phase 3 extended the frontend with organization management.

### Department Management

- Department list
- Department create
- Department edit
- Department delete
- Permission-controlled API access
- Audit/history tracking

### Position Management

- Position list
- Position create
- Position edit
- Position delete
- Department-based filtering
- Permission-controlled API access
- Audit/history tracking

### Phase 3 Verification

- React production build passes
- Backend build passes
- 57 automated tests pass
- CRUD operations manually tested
- Git repository clean and synchronized with GitHub

---

# Future Development

Additional enterprise modules may be added in future phases, including:

- User Management UI
- Role Management UI
- Permission Management UI
- Employee File Management UI
- Employee History / Audit UI
- Additional reporting and dashboard capabilities

These are intentionally kept as future work so the current project remains focused and maintainable.

---

# Project Goals

This project demonstrates practical experience with:

- Enterprise software architecture
- Modern .NET development
- Modern React development
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
