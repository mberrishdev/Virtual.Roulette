# Virtual Roulette API

A comprehensive .NET 8 virtual roulette game API built with Clean Architecture principles, providing authentication, real-time betting, jackpot tracking, and complete game history management.

## 🏗️ Architecture Overview

This solution follows Clean Architecture principles with clear separation of concerns across multiple layers:

### Core Layers
- **`Virtual.Roulette.Api`** - Presentation layer with ASP.NET Core Web API controllers, middleware, and Swagger documentation
- **`Virtual.Roulette.Application`** - Business logic layer with services, contracts, exceptions, and SignalR hubs  
- **`Virtual.Roulette.Domain`** - Core domain entities, primitives, validators, and domain exceptions
- **`Virtual.Roulette.Infrastructure`** - Infrastructure services and external integrations
- **`Virtual.Roulette.Persistence`** - Data access with Entity Framework Core and repository pattern (From common library)

### Testing
- **`Virtual.Roulette.Application.UnitTests`** - Unit tests with xUnit, FluentAssertions, and Moq

## 🎮 Key Features

### Authentication & Security
- JWT-based authentication with refresh tokens
- Secure password hashing
- User session tracking with automatic timeout (5 minutes) 

(In appsettings.json, refresh token expiration is set to 5 minutes, thats mean, after 5 minute user cannot access to the service, without login again. Auto logout, and redirect to auth page, should be done in frontend)
- IP address recording for all bets

### Betting System
- Comprehensive bet validation using `Ge.Singular.Roulette` package
- Secure random number generation for fair gameplay
- Real-time balance management with withdrawal/deposit operations
- Complete bet history tracking

### Real-time Features
- SignalR hub for live jackpot updates
- Real-time notifications to all connected clients
- Automatic jackpot calculation (1% of each bet)

### Game Management
- Roulette spin results and history
- User financial account management
- Comprehensive audit trail with timestamps and IP addresses

## 🛠️ Technology Stack

- **.NET 8**
- **ASP.NET Core**
- **Entity Framework Core**
- **SQL Server** 
- **SignalR**
- **JWT**
- **Swagger/OpenAPI**
- **xUnit**
- **Serilog**

### External Dependencies

- **`Ge.Singular.Roulette`** – Third-party package for roulette bet validation and win calculation.
- **[`BerrishDev.Common.Repository`](https://www.nuget.org/packages/BerrishDev.Common.Repository)** – A lightweight and flexible repository pattern implementation created by me.  
  📦 NuGet: `BerrishDev.Common.Repository`  
- **[`HubDocs`](https://www.nuget.org/packages/HubDocs)** – SignalR strongly-typed documentation generator and UI tooling, also developed by me.  
  📦 NuGet: `HubDocs`  
  🔗 GitHub: [github.com/mberrishdev/HubDocs](https://github.com/mberrishdev/HubDocs)


## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK or later
- SQL Server (LocalDB, Express, or full version)

### Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/mberrishdev/Virtual.Roulette
   cd Virtual.Roulette
   ```

2. **Configure the database**
   - Update the connection string in `src/Virtual.Roulette.Api/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "Virtual.Roulette": "Server=(local);Database=Virtual.Roulette;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

3. **Build and run**
   ```bash
   # Restore dependencies
   dotnet restore
   
   # Build the solution
   dotnet build --no-restore --configuration Release
   
   # Run the API
   dotnet run --project src/Virtual.Roulette.Api
   ```

4. **Access the application**
   - API: https://localhost:5001 or http://localhost:5000
   - Swagger UI: https://localhost:5001/swagger
   - HubDocs UI: https://localhost:5001/hubdocs

## 🧪 Testing

### Running Tests
```bash
# Run all unit tests
dotnet test --no-build --configuration Release

# Run tests with coverage
dotnet test --no-build --configuration Release --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test tests/Virtual.Roulette.Application.UnitTests --no-build --configuration Release
```

### Test Coverage
The project includes comprehensive unit tests covering:
- Authentication services (AuthService, JwtTokenService, RefreshTokenService)
- Account management (AccountService)
- Betting logic (BetService, BetValidator)
- Jackpot functionality (JackpotService)
- Spin management (SpinService)


## 📋 API Endpoints

### Authentication
- `POST /v1/auth/register` - Register a new user
- `POST /v1/auth/login` - Login and receive JWT token
- `POST /v1/auth/refresh` - Refresh access token

### Account Management
- `GET /v1/account/balance` - Get user's current balance

### Betting
- `POST /v1/bet` - Place a bet

### Game History
- `GET /v1/spin/history` - Get user's betting history

### Jackpot
- `GET /v1/jackpot` - Get current jackpot amount

### Real-time Updates
- SignalR Hub: `/jackpotHub` - Real-time jackpot updates

## 🏛️ Domain Models

### Core Entities
- **User** - User accounts and authentication
- **Account** - User financial accounts and balances  
- **Spin** - Roulette spin results and history
- **RefreshToken** - JWT refresh token management

## 🔐 Security Features

- Secure password hashing with salt
- JWT token-based authentication
- Refresh token rotation
- IP address logging for audit trails
- Input validation and sanitization
- SQL injection prevention through Entity Framework
- CORS configuration for cross-origin requests

## 📊 Performance Considerations

- Asynchronous operations throughout the application
- Repository pattern with Unit of Work for efficient data access
- Connection pooling through Entity Framework
- Structured logging with Serilog
- Health checks for monitoring

## 🐛 Development Tools

### Custom Tooling
This project leverages several custom development tools:

- **Template Project**: Bootstrapped using a custom .NET solution template - [GitHub](https://github.com/mberrishdev/Net.Template)
- **Repository & Unit of Work**: Data access managed by custom NuGet package - [NuGet](https://www.nuget.org/packages/BerrishDev.Common.Repository)
- **HubDocs**: API documentation generation - [GitHub](https://github.com/mberrishdev/HubDocs)

### Code Quality
- Comprehensive unit test coverage
- FluentAssertions for readable test assertions
- Moq for dependency mocking
- Code coverage reporting with Coverlet

## 📝 Assignment Requirements

This project fulfills all requirements from the Virtual Roulette assignment:

✅ JWT-based authentication with session tracking  
✅ User balance management in US dollar cents  
✅ Bet validation using RouletteBetAnalyzer library  
✅ Secure random number generation  
✅ Game history retrieval  
✅ Real-time jackpot updates via SignalR  
✅ IP address recording for audit trails  
✅ REST API principles  
✅ Comprehensive unit tests with xUnit  
✅ SQL Server database with Entity Framework  
✅ Swagger documentation  
✅ Clean architecture and coding standards  
✅ API versioning support