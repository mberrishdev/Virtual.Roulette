using Virtual.Roulette.Domain.Entities.Accounts;

namespace Virtual.Roulette.Application.Contracts.Services.AccountServices.Models;

public sealed record AccountModel
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public decimal Balance { get; init; }
    public int BalanceInCents { get; init; }
    public string Currency { get; init; } = "";

    public AccountModel()
    {
    }

    public AccountModel(Account account)
    {
        Id = account.Id;
        UserId = account.UserId;
        Balance = account.Balance;
        BalanceInCents = (int)Math.Round(account.Balance * 100, MidpointRounding.AwayFromZero);
        Currency = account.Currency;
    }
}