using Swashbuckle.AspNetCore.Filters;
using Virtual.Roulette.Application.Contracts.Services.AuthService.Models;

namespace Virtual.Roulette.Api.Swagger.Examples;

public class AuthRequestExample : IExamplesProvider<AuthRequest>
{
    public AuthRequest GetExamples()
    {
        return new AuthRequest
        {
            UserName = "john.doe",
            Password = "P@ssword123!"
        };
    }
} 