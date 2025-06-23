using ge.singular.roulette;
using Virtual.Roulette.Application.Contracts.Services.BetServices;

namespace Virtual.Roulette.Application.Services.BetServices;

public class BetValidator : IBetValidator
{
    public IsBetValidResponse IsValid(string betJson) => CheckBets.IsValid(betJson);
    public long EstimateWin(string betJson, int winningNumber) => CheckBets.EstimateWin(betJson, winningNumber);
}