# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Test Commands

### Building the solution
```bash
dotnet restore
dotnet build --no-restore --configuration Release
```

### Running tests
```bash
# Run all unit tests
dotnet test --no-build --configuration Release

# Run tests with coverage
dotnet test --no-build --configuration Release --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test tests/Virtual.Roulette.Application.UnitTests --no-build --configuration Release
```

### Running the application
```bash
# Apply database migrations
dotnet ef database update --project src/Virtual.Roulette.Api

# Run the API
dotnet run --project src/Virtual.Roulette.Api
```

## Architecture Overview

This is a .NET 8 virtual roulette game API following Clean Architecture principles with the following layers:

- **Api** (`Virtual.Roulette.Api`): Presentation layer with ASP.NET Core Web API controllers, middleware, and Swagger documentation
- **Application** (`Virtual.Roulette.Application`): Business logic layer with services, contracts, exceptions, and SignalR hubs  
- **Domain** (`Virtual.Roulette.Domain`): Core domain entities, primitives, validators, and domain exceptions
- **Infrastructure** (`Virtual.Roulette.Infrastructure`): Infrastructure services and external integrations
- **Persistence** (`Virtual.Roulette.Persistence`): Data access with Entity Framework Core and repository pattern

### Key Components

- **Authentication**: JWT-based auth with refresh tokens (`AuthService`, `JwtTokenService`, `RefreshTokenService`)
- **Betting System**: Bet validation and processing using the `Ge.Singular.Roulette` package for game logic
- **Real-time Updates**: SignalR hub (`JackpotHub`) for live jackpot updates
- **Data Access**: Uses `BerrishDev.Common.Repository` NuGet package implementing repository and unit of work patterns

### Domain Entities
- `User`: User accounts and authentication
- `Account`: User financial accounts and balances  
- `Spin`: Roulette spin results and history
- `RefreshToken`: JWT refresh token management

### External Dependencies
- `Ge.Singular.Roulette`: Third-party package for roulette bet validation and win calculation
- `BerrishDev.Common.Repository`: Custom repository pattern implementation
- `HubDocs`: API documentation generation

## Configuration

- Database connection string configured in `src/Virtual.Roulette.Api/appsettings.json` under `ConnectionStrings:Virtual.Roulette`
- Auth settings configured under `AuthSettings` section
- Requires SQL Server database

## Testing Framework

Uses xUnit with:
- `FluentAssertions` for readable assertions
- `Moq` for mocking dependencies
- Code coverage reports generated via `coverlet.collector`