using Virtual.Roulette.Application.Contracts.Services.JackpotServices.Models;

namespace Virtual.Roulette.Application.Contracts.Services.JackpotServices;

public interface IJackpotService
{
    Task AddToJackpot(decimal betAmount, CancellationToken cancellationToken);
    Task<decimal> GetJackpotAmountAsync(CancellationToken cancellationToken);
    Task BroadcastJackpotUpdateAsync(CancellationToken cancellationToken);
}