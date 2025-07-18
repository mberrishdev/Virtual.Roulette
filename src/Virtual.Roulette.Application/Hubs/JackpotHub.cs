using Microsoft.AspNetCore.SignalR;
using Virtual.Roulette.Application.Contracts.Services.JackpotServices;

namespace Virtual.Roulette.Application.Hubs;

public interface IJackpotClient
{
    Task ReceiveJackpot(decimal amount);
    Task JackpotUpdated(decimal amount);
}

public class JackpotHub(IJackpotService jackpotService) : Hub<IJackpotClient>
{
}