using Common.Repository.Repository;
using Microsoft.AspNetCore.SignalR;
using Virtual.Roulette.Application.Contracts.Services.JackpotServices;
using Virtual.Roulette.Application.Contracts.Services.JackpotServices.Models;
using Virtual.Roulette.Application.Hubs;
using Virtual.Roulette.Domain.Entities.Jackpots;

namespace Virtual.Roulette.Application.Services.JackpotServices;

public class JackpotService(
    IRepository<Jackpot> repository,
    IQueryRepository<Jackpot> queryRepository,
    IHubContext<JackpotHub, IJackpotClient> jackpotHubContext) : IJackpotService
{
    public async Task AddToJackpot(decimal betAmount, CancellationToken cancellationToken)
    {
        var amount = betAmount * 0.01m;
        await repository.InsertAsync(new Jackpot(amount), cancellationToken);
    }
    
    public async Task<decimal> GetJackpotAmountAsync(CancellationToken cancellationToken)
    {
        var jackpots = await queryRepository.GetListAsync(cancellationToken: cancellationToken);

        return jackpots.Sum(x => x.Amount);
    }

    public async Task BroadcastJackpotUpdateAsync(CancellationToken cancellationToken)
    {
        var currentAmount = await GetJackpotAmountAsync(cancellationToken);
        await jackpotHubContext.Clients.All.JackpotUpdated(currentAmount);
    }
}