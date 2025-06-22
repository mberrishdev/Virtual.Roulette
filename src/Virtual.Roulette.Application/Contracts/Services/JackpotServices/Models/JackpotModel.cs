namespace Virtual.Roulette.Application.Contracts.Services.JackpotServices.Models;

/// <summary>
/// Represents the current state of the jackpot.
/// </summary>
public class JackpotModel
{
    /// <summary>
    /// The current total amount of the jackpot.
    /// </summary>
    /// <example>10000.00</example>
    public decimal Amount { get; set; }
}