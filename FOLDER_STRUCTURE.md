# Folder Structure

## src

Contains all production source code.

---

## API

Responsibilities

- Controllers
- Middleware
- Authentication
- Dependency Injection
- Swagger
- Program.cs

No business logic should exist here.

---

## Application

Contains application use cases.

Responsibilities

- CQRS
- Commands
- Queries
- DTOs
- Validators
- Interfaces
- Behaviors

This layer coordinates business processes.

---

## Domain

Contains business rules.

Responsibilities

- Entities
- BaseEntity
- AuditableEntity
- HistoryEntity
- Value Objects
- Enums
- Domain Exceptions

The Domain project should have no dependency on any other project.

---

## Infrastructure

Contains external services.

Examples

- Email Service
- File Storage
- JWT
- Logging

---

## Persistence

Contains database implementation.

Responsibilities

- DbContext
- Entity Configurations
- Repository Implementations
- Migrations

---

## Tests

Contains unit and integration tests.
