using Virtual.Roulette.Application.Contracts.Services.AccountServices.Models;

namespace Virtual.Roulette.Application.Contracts.Services.AccountServices;

public interface IAccountService
{
    Task<AccountModel> GetAccount(int userId, CancellationToken cancellationToken);
    Task WithdrawAsync(int userId, decimal amount, CancellationToken cancellationToken);
    Task DepositAsync(int userId, decimal amount, CancellationToken cancellationToken);
}