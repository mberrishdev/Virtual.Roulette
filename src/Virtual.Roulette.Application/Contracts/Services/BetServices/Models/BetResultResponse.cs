namespace Virtual.Roulette.Application.Contracts.Services.BetServices.Models;

public class BetResultResponse
{
    public string Status { get; set; } = "Accepted";
    public Guid SpinId { get; set; }
    public int WinningNumber { get; set; }
    public decimal WonAmount { get; set; }
}