using Microsoft.AspNetCore.SignalR;
using Virtual.Roulette.Application.Contracts.Services.JackpotServices;
using Virtual.Roulette.Application.Contracts.Services.JackpotServices.Models;
using Virtual.Roulette.Application.Hubs;

namespace Virtual.Roulette.Application.Services.JackpotServices;

public class JackpotService(IHubContext<JackpotHub, IJackpotClient> jackpotHubContext) : IJackpotService
{
    private decimal _amount;
    private readonly object _lock = new();

    private decimal CurrentAmount
    {
        get
        {
            lock (_lock)
                return _amount;
        }
    }

    public JackpotModel GetJackpot()
    {
        return new JackpotModel()
        {
            Amount = CurrentAmount
        };
    }

    public decimal AddToJackpot(decimal betAmount)
    {
        var addition = betAmount * 0.01m;

        lock (_lock)
        {
            _amount += addition;

            return _amount;
        }
    }

    public void SetAmount(decimal amount)
    {
        lock (_lock)
        {
            _amount = amount;
        }
    }

    public async Task BroadcastJackpotUpdateAsync()
    {
        decimal currentAmount;
        lock (_lock)
        {
            currentAmount = _amount;
        }

        await jackpotHubContext.Clients.All.JackpotUpdated(currentAmount);
    }
}