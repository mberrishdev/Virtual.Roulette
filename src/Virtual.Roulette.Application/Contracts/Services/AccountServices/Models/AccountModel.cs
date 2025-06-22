namespace Virtual.Roulette.Application.Contracts.Services.AccountServices.Models;

public sealed record AccountModel
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public decimal Balance { get; init; }
    public int BalanceInCents { get; init; }
    public required string Currency { get; init; }
}