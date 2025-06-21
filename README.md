# Virtual Roulette

This project is a backend API for a virtual roulette game, built with .NET 8 and following clean architecture principles.

## Architecture

The solution is structured into the following projects, adhering to a clean architecture design:

*   **`Virtual.Roulette.Api`**: Presentation layer (ASP.NET Core Web API).
*   **`Virtual.Roulette.Application`**: Application layer (business logic, services, models, hubs).
*   **`Virtual.Roulette.Domain`**: Domain layer (entities, core business rules).
*   **`Virtual.Roulette.Infrastructure`**: Infrastructure layer.
*   **`Virtual.Roulette.Persistence`**: Data access layer (Entity Framework Core / Repository pattern & Unit of work).

## Custom Tooling

This project was built using a suite of custom tools to accelerate development and ensure consistency:

*   **Template Project**: This project was bootstrapped using a custom .NET solution template. [Github](https://github.com/mberrishdev/Net.Template)
*   **Repository & Unit of Work**: Data access is managed by a custom NuGet package that implements the repository and unit of work patterns. (source is private, I cannot make it public, contact me and i will show you ) [NuGet package](https://www.nuget.org/packages/BerrishDev.Common.Repository) 
*   **HubDocs**: Documentation is handled by the `HubDocs` NuGet package. [Github](https://github.com/mberrishdev/HubDocs)

## Getting Started

### Prerequisites

*   .NET 8 SDK
*   SQL Server

### Configuration

1.  **Connection String**: Update the connection string in `src/Virtual.Roulette.Api/appsettings.json`.

### Running the Application

1.  Clone the repository.
2.  Apply database migrations: `dotnet ef database update`.
3.  Run the API: `dotnet run`.
