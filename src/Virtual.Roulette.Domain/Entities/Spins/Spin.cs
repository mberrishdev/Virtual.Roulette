using Virtual.Roulette.Domain.Entities.Users;
using Virtual.Roulette.Domain.Primitives;

namespace Virtual.Roulette.Domain.Entities.Spins;

/// <summary>
/// Represents a single spin of the roulette wheel.
/// </summary>
public class Spin : Entity<Guid>
{
    public int UserId { get; private set; }

    public string BetString { get; private set; } = default!;
    public long BetAmountCents { get; private set; }

    public long WonAmountCents { get; private set; }
    public int WinningNumber { get; private set; }
    public string IpAddress { get; private set; } = default!;
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = default!;

    private Spin()
    {
        
    }
    
    public Spin(int userId, string betString, long betAmountCents, int winningNumber, long wonAmountCents, string ipAddress)
    {
        UserId = userId;
        BetString = betString;
        BetAmountCents = betAmountCents;
        WinningNumber = winningNumber;
        WonAmountCents = wonAmountCents;
        IpAddress = ipAddress;
        CreatedAt = DateTime.UtcNow;
    }
}