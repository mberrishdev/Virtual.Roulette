using ge.singular.roulette;

namespace Virtual.Roulette.Application.Contracts.Services.BetServices;

public interface IBetValidator
{
    IsBetValidResponse IsValid(string betJson);
    long EstimateWin(string betJson, int winningNumber);
}