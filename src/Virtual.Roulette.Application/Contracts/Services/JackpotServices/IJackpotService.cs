namespace Virtual.Roulette.Application.Contracts.Services.JackpotServices;

public interface IJackpotService
{
    decimal CurrentAmount { get; }
    decimal AddToJackpot(decimal betAmount);
    void SetAmount(decimal amount);
    Task BroadcastJackpotUpdateAsync();
}