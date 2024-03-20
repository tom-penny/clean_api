# Shop API

## Overview

Shop API is a comprehensive solution designed to address the needs of modern e-commerce platforms; fulfilling a range of use cases, from user management and authorisation, to product cataloguing, order processing, and shipment tracking. Built in .NET Core, the API leverages cutting-edge principles such as Domain-Driven Design (DDD), Command Query Responsibility Segregation (CQRS), and Clean Architecture to facilitate a robust, scalable, maintainable system, capable of accommodating future growth and technological advancement.

### Domain-Driven Design (DDD)

The API's architecture is deeply rooted in DDD principles, organising the system around the business domain and logic. This approach fosters a ubiquitous language, encapsulates complex business rules, and facilitates a modular system design, accommodating future growth and adaptability.

### Command Query Responsibility Segregation (CQRS)

In accordance with CQRS, the application logic is bifurcated into dedicated commands and queries, enhancing the performance of the system by allowing read and write operations to scale independently.

### Clean Architecture

Following Clean Architecture principles, Shop API is structured into concentric layers—API, Application, Domain, Infrastructure—each with distinct responsibilities. This stratification decouples business logic from infrastructure concerns, promoting maintainability by ensuring that the application’s core remains unaffected by changes in external frameworks, databases, and configurations.

## Key Features

- **RESTful Endpoints** - CRUD operations for managing products, categories, orders, payments, and users, adhering to RESTful design principles.
- **Event-Driven Architecture** - Leverages domain events to decouple service interactions and factiliate asynchronous communication.
- **Transactional Outbox** - Employs the outbox pattern to maintain consistency across distributed transactions.
- **Order Saga Management** - Leverages event choreography to facilitate long-lived order transactions and workflows, ensuring data consistency and integrity across service boundaries.
- **Authentication & Authorisation** - Integrates ASP.NET Core Identity with JSON Web Tokens (JWT) for secure authentication and fine-grained authorisation controls.
- **In-Memory Caching** - Implements in-memory caching for frequently accessed data, minimising database loads and optimising response times for common read operations.
- **Email Notifications** - Features an integrated email service for user and order event notifications.
- **Unit & Integration Testing** - Includes automated testing suites using xUnit, Moq, Fluent Assertions, Testcontainers, and Respawn.

## Project Structure

- `Shop.API` - Contains the entry point of the application, controllers, middleware, models, and DTOs for handling HTTP requests and responses.
- `Shop.Application` - Defines the application's core logic, including commands, queries, and domain event handlers.
- `Shop.Domain` - Defines the domain entities, enums, errors, and events, encapsulating the business logic and rules.
- `Shop.Infrastructure` - Responsible for external concerns, such as database access, messaging, caching, and authentication.
- `Shop.API.IntegrationTests` - Contains integration tests for testing the API endpoints.
- `Shop.Application.UnitTests` - Comprises unit tests for the application layer.
- `Shop.Domain.UnitTests` - Contains unit tests for domain entities and logic.

## Getting Started

### Prerequisites

- .NET Core 8.0 SDK
- PostgreSQL server

### Setup

1. Clone the repository:

    ```git clone https://github.com/w1855014/shop-backend.git```

2. Install dependencies from the solution directory:

    ```dotnet restore```

3. Configure the PostgreSQL connection string in appsettings.json.

4. Run the application from the solution directory:

    ```dotnet run Shop.API```

### Testing

- Run `Shop.Application.UnitTests` from the solution directory:

    ```dotnet test Shop.Application.UnitTests```

- Run `Shop.Domain.UnitTests` from the solution directory:

    ```dotnet test Shop.Domain.UnitTests```

- Run `Shop.API.InfrastructureTests` from the solution directory:

    ```dotnet test Shop.API.InfrastructureTests```

## Technologies

* [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core) - A framework for building high-performance, cross-platform web APIs.
* [ASP .NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity) - A framework for implementing user identity and security features.
* [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) - An object-relational mapping framework with a fluent API for accessing databases.
* [PostgreSQL](https://www.postgresql.org/) - A powerful, open-source relational database system.
* [Mediatr](https://github.com/jbogard/MediatR) A simple Mediator implementation for in-process messaging and request handling.
* [Fluent Results](https://github.com/altmann/FluentResults) - A library for encapsulating functional outcomes, represented as success and failure states.
* [Fluent Validation](https://fluentvalidation.net/) - A library for building strongly-typed validation rules.
* [xUnit](https://xunit.net/) - A framework for building automated tests.
* [Moq](https://github.com/moq) - A mocking framework for isolating test components with simulated dependencies.
* [Fluent Assertions](https://fluentassertions.com/) - A library of fluent extension methods for asserting functional outcomes.
* [Testcontainers](https://dotnet.testcontainers.org/) - A library for provisioning tests with throwaway Docker containers.
* [Respawn](https://github.com/jbogard/Respawn) - A lightweight utility for resetting database state between tests.

## License

This project is licensed under the [MIT License]().

## Acknowledgements

* [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) - Robert C. Martin.
* [Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture) - @jasontaylordev.
* [CQRS Documents](https://cqrs.files.wordpress.com/2010/11/cqrs_documents.pdf) - Greg Young.
* [Domain-Driven Design: Tackling Complexity in the Heart of Software](https://www.amazon.com/dp-0321125215/dp/0321125215/) - Eric Evans.