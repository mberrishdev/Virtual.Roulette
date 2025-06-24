using Virtual.Roulette.Domain.Entities.Spins;

namespace Virtual.Roulette.Application.Contracts.Services.SpinServices.Models;

public sealed record SpinModel
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string BetString { get; set; } = "";
    public long BetAmountCents { get; set; }
    public long WonAmountCents { get; set; }
    public int WinningNumber { get; set; }
    public string IpAddress { get; set; } = "";
    public DateTime CreatedAt { get; set; }

    public SpinModel()
    {
    }

    public SpinModel(Spin spin)
    {
        Id = spin.Id;
        UserId = spin.UserId;
        BetString = spin.BetString;
        BetAmountCents = spin.BetAmountCents;
        WonAmountCents = spin.WonAmountCents;
        WinningNumber = spin.WinningNumber;
        IpAddress = spin.IpAddress;
        CreatedAt = spin.CreatedAt;
    }
}