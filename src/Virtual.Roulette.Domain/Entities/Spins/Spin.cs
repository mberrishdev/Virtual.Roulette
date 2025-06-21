using Virtual.Roulette.Domain.Entities.Users;
using Virtual.Roulette.Domain.Primitives;

namespace Virtual.Roulette.Domain.Entities.Spins;

public class Spin : Entity<Guid>
{
    public int UserId { get; set; }
    public string BetString { get; set; } = default!;
    public long BetAmountCents { get; set; }
    public long WonAmountCents { get; set; }
    public int WinningNumber { get; set; }
    public string IpAddress { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    
    public User User { get; set; } = default!;
}