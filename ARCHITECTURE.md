# Architecture

This project follows the principles of **Clean Architecture**.

The main objective is to separate responsibilities, reduce coupling, improve maintainability, and keep core business concepts independent from frameworks, databases, and external technologies.

---

# Architecture Diagram

```text
                    API
                     │
                     ▼
               Application
                 │       │
                 ▼       ▼
              Domain   Abstractions
                 ▲       ▲
                 │       │
        Persistence   Infrastructure
```

The **Domain** layer remains independent of the other projects.

The **Application** layer contains use cases and abstractions.

**Persistence** and **Infrastructure** provide implementations for application and external concerns.

The **API** layer is responsible for HTTP endpoints, authentication/authorization configuration, middleware, Swagger/OpenAPI, dependency injection, and application startup.

---

# Project Responsibilities

## API

The API project is responsible for exposing REST endpoints and handling HTTP concerns.

Responsibilities:

- Controllers
- Middleware
- Authentication configuration
- Authorization configuration
- Swagger / OpenAPI
- Dependency Injection
- Health Checks
- Request / Response handling
- Program.cs / Application startup

The API project should NOT contain business logic.

---

## Application

The Application project contains application use cases and business process orchestration.

Responsibilities:

- Commands
- Queries
- CQRS
- MediatR
- DTOs
- Validators
- Interfaces
- MediatR Pipeline Behaviors
- Application services

This layer coordinates application workflows.

It knows what needs to happen but does not know how data is stored or how external services are implemented.

---

## Domain

The Domain project contains the core business entities and domain concepts.

Responsibilities:

- Business Entities
- BaseEntity
- AuditableEntity
- HistoryEntity
- Business Rules
- Value Objects
- Enums
- Domain Exceptions

The Domain project must remain independent of:

- Entity Framework Core
- SQL Server
- ASP.NET Core
- HTTP
- Controllers
- External services

It should contain core business concepts rather than infrastructure or framework-specific implementation details.

---

## Persistence

The Persistence project handles database access and persistence implementation.

Responsibilities:

- DbContext
- Entity Configurations
- Repository Implementations
- EF Core Migrations
- Database Seeding
- SQL Server Configuration

Persistence is responsible for Entity Framework Core and database-specific implementation details.

Only the persistence layer communicates directly with the database.

---

## Infrastructure

Infrastructure contains implementations for external and cross-cutting services.

Responsibilities / Examples:

- JWT / Authentication Services
- Password Hashing
- File Storage
- External Services
- Logging-related infrastructure
- Third-party API integrations

Infrastructure implements application abstractions where appropriate.

These are implementation details outside the core business domain.

---

## Tests

The test project contains automated tests used to validate application behavior.

Current test coverage includes:

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

# Domain Entity Hierarchy

```text
BaseEntity
      │
      ├── AuditableEntity
      │
      └── HistoryEntity
```

### BaseEntity

Contains properties shared by entities.

```text
Id
StatusCode
```

---

### AuditableEntity

Adds audit information for entities that require audit tracking.

```text
CreatedBy
CreatedAt
UpdatedBy
UpdatedAt
```

---

### HistoryEntity

Contains information describing a historical action.

```text
ActionTypeCode
ActionBy
ActionAt
```

History entities use this information to record changes to business records.

---

# Audit Strategy

This project uses **Snapshot Audit History**.

Instead of recording only changed columns, a history record stores a complete snapshot of the relevant record together with action information.

Advantages:

- Simple reporting
- Easier debugging
- Full historical snapshot
- Easier SQL queries
- Clear audit trail

Example:

```text
Employee

John
IT
Developer

        │
        ▼
Update Department
        │
        ▼

EmployeeHistory

John
HR
Developer
ActionType = Update
```

---

# Lookup Strategy

The application uses stable **Lookup Codes** for code-based values.

Example:

Instead of relying on a human-readable display value:

```text
Gender = Male
```

The system stores a stable code:

```text
GenderCode = "M"
```

Advantages:

- Stable values
- Easier SQL queries
- Better readability
- Simpler reporting
- Display names can change without changing the stored code

Example:

```text
Code = M

Display Name:
Male
```

The application treats the code as the stable business value while the display name can be changed independently.

---

# Security Architecture

The application uses JWT Bearer Authentication combined with role and permission-based authorization.

Security responsibilities include:

- JWT Bearer Authentication
- Claims-Based Authentication
- Role-Based Authorization
- Permission-Based Authorization
- Dynamic Permission Policies
- Custom Authorization Handling
- Password Hashing
- Standardized 401 / 403 responses

Permission policies are evaluated dynamically based on the authenticated user's permissions.

---

# Application Request Flow

A typical API request follows this flow:

```text
HTTP Request
     │
     ▼
Controller
     │
     ▼
MediatR
     │
     ▼
ValidationBehavior
     │
     ▼
Validator
     │
     ▼
Handler
     │
     ▼
Repository / Application Service
     │
     ▼
Persistence
     │
     ▼
SQL Server
```

Cross-cutting concerns such as authentication, authorization, validation, and exception handling are applied through the appropriate middleware, authorization handlers, and MediatR pipeline behaviors.

---

# Design Principles

This project follows:

- Clean Architecture
- SOLID Principles
- Separation of Concerns
- Single Responsibility Principle
- Dependency Inversion Principle
- DRY (Don't Repeat Yourself)
- KISS (Keep It Simple)

---

# Current Status

Version **v1.0.0** has been completed as the production-ready portfolio milestone.

Completed capabilities include:

- Employee Management
- User Management
- Authentication
- Authorization
- Role & Permission Management
- Audit / History Tracking
- File Upload / Download
- Automated Testing
- Docker
- Docker Compose
- GitHub Actions CI/CD
- GitHub Container Registry
- Azure App Service
- Azure SQL
- Health Checks
- Swagger / OpenAPI

---

# Phase 2 — Planned

The next development phase is planned to extend the system with a modern frontend and additional capabilities.

Planned items include:

- React frontend
- Bootstrap 5 UI
- API integration
- Frontend authentication
- Improved user experience
- Additional cloud / storage enhancements

---

# Long-Term Goal

The project is intended to demonstrate production-oriented enterprise software development using modern .NET technologies.

The goal is to continue improving the system while maintaining:

- Clean architecture
- Maintainable code
- Secure APIs
- Automated testing
- CI/CD practices
- Containerization
- Cloud deployment
- Clear separation of concerns
