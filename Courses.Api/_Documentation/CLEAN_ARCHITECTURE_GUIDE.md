# Clean Architecture Guide

This document outlines the Clean Architecture structure of the Courses Platform API, focusing on how we apply architectural principles for a scalable, maintainable, and testable system.

## Core Principles

The architecture is centered around a separation of concerns, dividing the software into layers. The core principle is the **Dependency Rule**, which states that source code dependencies can only point inwards. Nothing in an inner layer can know anything at all about an outer layer.

## Project Structureد 

The solution is divided into the following projects, representing the layers of the architecture:

- `Courses.Domain`: The innermost layer, containing enterprise-wide business logic, models, and entities. It has no dependencies on any other project in the solution.
- `Courses.Application`: This layer contains the application-specific business logic. It orchestrates the data flow between the Domain and the outer layers.
- `Courses.Infrastructure`: This layer handles external concerns like databases, email services, and other third-party integrations. It implements the interfaces defined in the Application layer.
- `Courses.Api`: The outermost layer, which exposes the application's functionality via a RESTful API. It handles HTTP requests, authentication, and presentation logic.
- `Courses.Shared`: A shared library containing DTOs, Enums, custom exceptions, and other cross-cutting concerns that are used across multiple projects.

## Application Layer (CQRS Implementation)

The `Courses.Application` layer is designed following the **Command Query Responsibility Segregation (CQRS)** pattern. This pattern separates read and update operations for a data store.

### Key Characteristics:

- **MediatR:** We use the `MediatR` library to implement the CQRS pattern. It acts as a mediator to dispatch Commands and Queries to their respective handlers.
- **Features Folder:** All application logic is organized into "features". Each feature represents a distinct use case or a group of related use cases (e.g., `Authentication`, `Courses`).
- **Commands:** Commands represent an intent to change the state of the system (e.g., `StudentRegisterCommand`). They are handled by `CommandHandler` classes and do not return data, or return a simple acknowledgement DTO.
- **Queries:** Queries are used to read and retrieve data from the system without changing its state (e.g., `LoginQuery`). They are handled by `QueryHandler` classes and return DTOs.
- **Handlers:** Each command and query has a dedicated handler that encapsulates the business logic for that specific operation. Handlers are responsible for interacting with the domain, using infrastructure services, and returning the result.

### Benefits of this Approach:

1.  **Separation of Concerns:** The logic for reading data is clearly separated from the logic for writing data.
2.  **Scalability:** We can scale the read and write models independently.
3.  **Single Responsibility Principle:** Each handler has a single, well-defined responsibility.
4.  **Maintainability:** The code is easier to understand, maintain, and test. New features can be added by simply creating new commands/queries and their handlers without modifying existing code.

## Data Flow (Example: Student Registration)

1.  **Controller:** `StudentAuthController` receives an HTTP POST request on `/api/auth/student/register`.
2.  **MediatR:** The controller creates a `StudentRegisterCommand` and sends it using the `IMediator` interface.
3.  **Command Handler:** `StudentRegisterCommandHandler` receives the command.
4.  **Business Logic:** The handler executes the registration logic:
    -   Checks if the user already exists using `UserManager`.
    -   Creates a new `ApplicationUser` entity.
    -   Uses `UserManager` to save the new user to the database.
    -   Calls `IEmailService` and `ITwoFactorService` to send emails/codes.
5.  **Response:** The handler returns a `RegisterResponseDto`, which is then sent back to the client by the controller.

This approach ensures that our controllers are lean and only responsible for routing requests and formatting responses, while the core business logic resides within the application layer, completely decoupled from the presentation layer.
