using Swashbuckle.AspNetCore.Filters;
using Virtual.Roulette.Application.Contracts.Services.BetServices.Models;

namespace Virtual.Roulette.Api.Swagger.Examples;

public class BetRequestExample : IExamplesProvider<BetRequest>
{
    public BetRequest GetExamples()
    {
        return new BetRequest
        {
            BetJson = "[{\"T\":\"v\",\"I\":10,\"C\":1,\"K\":1},{\"T\":\"n\",\"I\":11,\"C\":1,\"K\":1},{\"T\":\"n\",\"I\":8,\"C\":1,\"K\":1},{\"T\":\"n\",\"I\":5,\"C\":6,\"K\":1},{\"T\":\"n\",\"I\":19,\"C\":1,\"K\":1},{\"T\":\"n\",\"I\":16,\"C\":1,\"K\":1},{\"T\":\"n\",\"I\":14,\"C\":1,\"K\":1},{\"T\":\"s\",\"I\":18,\"C\":1,\"K\":1}]"
        };
    }
} 