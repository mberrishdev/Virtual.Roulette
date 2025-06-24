using Swashbuckle.AspNetCore.Filters;
using Virtual.Roulette.Application.Contracts.Services.AuthServices.Models;

namespace Virtual.Roulette.Api.Swagger.Examples;

public class RegisterRequestExample : IExamplesProvider<RegisterRequest>
{
    public RegisterRequest GetExamples()
    {
        return new RegisterRequest
        {
            Username = "userName",
            Password = "password"
        };
    }
} 