using Common.Repository.Repository;
using Virtual.Roulette.Application.Contracts.Services.AccountServices;
using Virtual.Roulette.Application.Contracts.Services.AccountServices.Models;
using Virtual.Roulette.Application.Exceptions;
using Virtual.Roulette.Domain.Entities.Accounts;
using Virtual.Roulette.Domain.Entities.Users;

namespace Virtual.Roulette.Application.Services.AccountServices;

public class AccountService(IQueryRepository<Account> accountQueryRepository, IRepository<Account> accountRepository)
    : IAccountService
{
    public async Task<AccountModel> GetAccount(int userId, CancellationToken cancellationToken)
    {
        var account = await accountQueryRepository.GetAsync(a => a.UserId == userId, cancellationToken: cancellationToken)
                      ?? throw new ObjectNotFoundException(nameof(Account), nameof(Account.UserId), userId);

        return new AccountModel
        {
            Id = account.Id,
            UserId = account.UserId,
            Balance = account.Balance,
            BalanceInCents = (int)Math.Round(account.Balance * 100, MidpointRounding.AwayFromZero),
            Currency = account.Currency,
        };
    }

    public async Task WithdrawAsync(int userId, decimal amount, CancellationToken cancellationToken)
    {
        var account =
            await accountRepository.GetForUpdateAsync(a => a.UserId == userId, cancellationToken: cancellationToken)
            ?? throw new ObjectNotFoundException(nameof(Account), nameof(Account.UserId), userId);
        
        account.Withdraw(amount);
        await accountRepository.UpdateAsync(account, cancellationToken);
    }

    public async Task DepositAsync(int userId, decimal amount, CancellationToken cancellationToken)
    {
        var account =
            await accountRepository.GetForUpdateAsync(a => a.UserId == userId, cancellationToken: cancellationToken)
            ?? throw new ObjectNotFoundException(nameof(Account), nameof(Account.UserId), userId);

        account.Deposit(amount);
        await accountRepository.UpdateAsync(account, cancellationToken);
    }
}