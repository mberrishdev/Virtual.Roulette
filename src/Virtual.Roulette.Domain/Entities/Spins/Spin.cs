using Virtual.Roulette.Domain.Entities.Users;
using Virtual.Roulette.Domain.Primitives;

namespace Virtual.Roulette.Domain.Entities.Spins;

/// <summary>
/// Represents a single spin of the roulette wheel.
/// </summary>
public class Spin : Entity<Guid>
{
    /// <summary>
    /// The ID of the user who made the spin.
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// The JSON representation of the bet placed.
    /// </summary>
    public string BetString { get; set; } = default!;
    /// <summary>
    /// The amount bet, in cents.
    /// </summary>
    public long BetAmountCents { get; set; }
    /// <summary>
    /// The amount won, in cents.
    /// </summary>
    public long WonAmountCents { get; set; }
    /// <summary>
    /// The winning number of the spin.
    /// </summary>
    public int WinningNumber { get; set; }
    /// <summary>
    /// The IP address of the user who made the spin.
    /// </summary>
    public string IpAddress { get; set; } = default!;
    /// <summary>
    /// The timestamp of when the spin was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    public User User { get; set; } = default!;
}