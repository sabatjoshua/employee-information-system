# Architecture

This project follows the principles of **Clean Architecture**.

The main objective is to separate responsibilities, reduce coupling, improve maintainability, and allow the business logic to remain independent of frameworks and external technologies.

---

# Architecture Diagram

```
                    API
                     │
                     ▼
               Application
                     │
                     ▼
                  Domain
                ▲         ▲
                │         │
        Persistence   Infrastructure
```

The dependency always points **towards the Domain**.

The Domain layer never depends on any other project.

---

# Project Responsibilities

## API

The API project is responsible for exposing REST endpoints.

Responsibilities:

- Controllers
- Middleware
- Authentication
- Authorization
- Swagger
- Dependency Injection
- Request / Response

This project should NOT contain business logic.

---

## Application

The Application project contains application use cases.

Responsibilities:

- Commands
- Queries
- CQRS
- MediatR
- DTOs
- Validators
- Interfaces
- Behaviors

This layer coordinates business processes.

It knows what needs to happen but does not know how data is stored.

---

## Domain

The Domain project is the heart of the application.

Responsibilities:

- Business Entities
- BaseEntity
- AuditableEntity
- HistoryEntity
- Business Rules
- Value Objects
- Enums
- Domain Exceptions

The Domain project must never reference:

- Entity Framework
- SQL Server
- ASP.NET Core
- HTTP
- Controllers

It should contain only business concepts.

---

## Persistence

The Persistence project handles all database operations.

Responsibilities:

- DbContext
- Entity Configurations
- Repository Implementations
- Migrations
- SQL Server

Only this project communicates directly with the database.

---

## Infrastructure

Infrastructure contains external services.

Examples:

- JWT Authentication
- File Storage
- Email Service
- Logging
- Azure Blob Storage
- Third-party APIs

These are implementation details outside the business domain.

---

## Tests

Contains:

- Unit Tests
- Integration Tests

Every important business rule should eventually have corresponding tests.

---

# Domain Entity Hierarchy

```
BaseEntity
      │
      ▼
AuditableEntity
      │
      ▼
HistoryEntity
```

### BaseEntity

Contains properties shared by every entity.

```
Id
StatusCode
```

---

### AuditableEntity

Adds audit information.

```
CreatedBy
CreatedAt
UpdatedBy
UpdatedAt
```

---

### HistoryEntity

Adds history information.

```
ActionTypeCode
ActionBy
ActionAt
```

Every History table inherits from this class.

---

# Audit Strategy

This project uses **Snapshot Audit History**.

Instead of recording only changed columns, every change stores a complete copy of the record.

Advantages:

- Simple reporting
- Easier debugging
- Easier rollback
- Full historical snapshot
- Easier SQL queries

Example:

```
Employee

John
IT
Developer

↓

Update Department

↓

EmployeeHistory

John
HR
Developer
ActionType = Update
```

---

# Lookup Strategy

Instead of storing Lookup IDs, this project stores Lookup Codes.

Example:

Instead of

```
GenderId = GUID
```

The system stores

```
GenderCode = "M"
```

Advantages:

- Easier SQL queries
- Better readability
- Enum support in C#
- Simpler reporting

The code never changes.

Only the display name can be modified.

Example:

```
Code = M

Display Name

Male
Man
Masculine
```

The database stores only the stable code.

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

# Long-term Goal

By version **1.0.0**, this project will include:

- Complete Employee Management
- Authentication
- Authorization
- Role Management
- Audit Trail
- File Upload
- Docker
- GitHub Actions
- Azure Deployment

This project is intended to represent production-quality enterprise software development using modern .NET technologies.
