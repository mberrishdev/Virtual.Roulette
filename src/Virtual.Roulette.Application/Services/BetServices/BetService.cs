using System.Security.Cryptography;
using Common.Repository.UnitOfWork;
using Virtual.Roulette.Application.Contracts.Services.AccountServices;
using Virtual.Roulette.Application.Contracts.Services.BetServices;
using Virtual.Roulette.Application.Contracts.Services.BetServices.Models;
using Virtual.Roulette.Application.Contracts.Services.JackpotServices;
using Virtual.Roulette.Application.Contracts.Services.SpinServices;
using Virtual.Roulette.Domain.Entities.Spins;

namespace Virtual.Roulette.Application.Services.BetServices;

public class BetService(
    ISpinService spinService,
    IAccountService accountService,
    IJackpotService jackpotService,
    IBetValidator betValidator,
    IUnitOfWork unitOfWork)
    : IBetService
{
    public async Task<BetResultResponse> PlaceBetAsync(int userId, string betJson, string ipAddress,
        CancellationToken cancellationToken)
    {
        var account = await accountService.GetAccount(userId, cancellationToken);

        var validation = betValidator.IsValid(betJson);
        if (!validation.getIsValid())
            throw new Exception("Bet is invalid");

        var betAmount = validation.getBetAmount();

        if (account.Balance < betAmount)
            throw new Exception("Insufficient balance");

        var winningNumber = RandomNumberGenerator.GetInt32(0, 37);
        var wonAmountInCents = betValidator.EstimateWin(betJson, winningNumber);
        var wonAmount = wonAmountInCents / 100.0m;

        using var scope = await unitOfWork.CreateScopeAsync(cancellationToken);

        await accountService.WithdrawAsync(userId, betAmount, cancellationToken);

        if (wonAmount > 0)
            await accountService.DepositAsync(userId, wonAmount, cancellationToken);

        var spinId = await spinService.CreateSpinAsync(new Spin
        {
            UserId = userId,
            BetString = betJson,
            BetAmountCents = betAmount,
            WonAmountCents = wonAmountInCents,
            WinningNumber = winningNumber,
            IpAddress = ipAddress
        }, cancellationToken);

        await scope.CompletAsync(cancellationToken);

        jackpotService.AddToJackpot(wonAmountInCents / 100.0m);
        await jackpotService.BroadcastJackpotUpdateAsync();

        return new BetResultResponse
        {
            SpinId = spinId,
            WinningNumber = winningNumber,
            WonAmount = wonAmountInCents
        };
    }
}