namespace Virtual.Roulette.Application.Contracts.Services.AccountServices.Models;

public class AccountModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Balance { get; set; }
    public int BalanceInCents { get; set; } 
    public required string Currency { get; set; }
}