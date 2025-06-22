namespace Virtual.Roulette.Application.Contracts.Services.BetServices.Models;

/// <summary>
/// Represents a request to place a bet.
/// </summary>
public class BetRequest
{
    /// <summary>
    /// A JSON string representing the bet details.
    /// </summary>
    /// <example>{"type":"single","number":10,"amount":100}</example>
    public required string BetJson { get; set; }
}