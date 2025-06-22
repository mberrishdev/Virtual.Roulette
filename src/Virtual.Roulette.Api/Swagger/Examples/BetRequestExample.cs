using Swashbuckle.AspNetCore.Filters;
using Virtual.Roulette.Application.Contracts.Services.BetServices.Models;

namespace Virtual.Roulette.Api.Swagger.Examples;

public class BetRequestExample : IExamplesProvider<BetRequest>
{
    public BetRequest GetExamples()
    {
        return new BetRequest
        {
            BetJson = "{\"type\":\"single\",\"number\":10,\"amount\":100}"
        };
    }
} 