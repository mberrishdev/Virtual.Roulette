using Virtual.Roulette.Application.Contracts.Services.BetServices.Models;

namespace Virtual.Roulette.Application.Contracts.Services.BetServices;

public interface IBetService
{
    Task<BetResultResponse> PlaceBetAsync(int userId, string betJson, string ipAddress,
        CancellationToken cancellationToken);
}