# Folder Structure

## src

Contains all production source code.

---

## API

Responsibilities

* Controllers
* Middleware
* Authentication configuration
* Authorization configuration
* Dependency Injection
* Swagger / OpenAPI
* Health Checks
* Program.cs

The API layer handles HTTP concerns and application startup. Business logic should not be implemented here.

---

## Application

Contains application use cases and business process orchestration.

Responsibilities

* CQRS
* Commands
* Queries
* DTOs
* Validators
* Interfaces
* MediatR Pipeline Behaviors
* Application Services

This layer coordinates application workflows and depends on abstractions rather than infrastructure implementations.

---

## Domain

Contains core business entities and domain concepts.

Responsibilities

* Entities
* BaseEntity
* AuditableEntity
* HistoryEntity
* Value Objects
* Enums
* Domain Exceptions

The Domain project should remain independent of infrastructure, persistence, and API concerns.

---

## Infrastructure

Contains implementations for external and cross-cutting services.

Responsibilities / Examples

* JWT / Authentication Services
* Password Hashing
* File Storage Services
* External Services
* Logging-related infrastructure

Infrastructure implements interfaces defined by the Application layer where appropriate.

---

## Persistence

Contains database access and persistence implementation.

Responsibilities

* DbContext
* Entity Configurations
* Repository Implementations
* Database Migrations
* Database Seeding
* SQL Server Configuration

Persistence is responsible for Entity Framework Core and database-specific implementation details.

---

## Tests

Contains automated tests for the application.

Responsibilities

* Unit Tests
* Application Tests
* Validation Tests
* Authentication / Authorization Tests
* Exception Handling Tests
* CRUD Tests
* File Upload / Download Tests

The test project validates application behavior and helps ensure changes do not introduce regressions.
