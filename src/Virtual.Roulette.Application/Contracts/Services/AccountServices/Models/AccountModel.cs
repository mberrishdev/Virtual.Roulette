namespace Virtual.Roulette.Application.Contracts.Services.AccountServices.Models;

/// <summary>
/// Represents a user's account details.
/// </summary>
public class AccountModel
{
    /// <summary>
    /// The unique identifier for the account.
    /// </summary>
    /// <example>101</example>
    public int Id { get; set; }

    /// <summary>
    /// The unique identifier for the user who owns the account.
    /// </summary>
    /// <example>1</example>
    public int UserId { get; set; }

    /// <summary>
    /// The account balance.
    /// </summary>
    /// <example>500.00</example>
    public decimal Balance { get; set; }

    public int BalanceInCents { get; set; } 
    public required string Currency { get; set; }
}