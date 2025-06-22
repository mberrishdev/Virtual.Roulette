using Virtual.Roulette.Application.Contracts.Services.JackpotServices.Models;

namespace Virtual.Roulette.Application.Contracts.Services.JackpotServices;

public interface IJackpotService
{
    public JackpotModel GetJackpot();
    decimal AddToJackpot(decimal betAmount);
    void SetAmount(decimal amount);
    Task BroadcastJackpotUpdateAsync();
}