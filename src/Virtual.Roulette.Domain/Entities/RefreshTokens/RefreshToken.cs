using Virtual.Roulette.Domain.Entities.Users;
using Virtual.Roulette.Domain.Primitives;

namespace Virtual.Roulette.Domain.Entities.RefreshTokens;

public class RefreshToken : Entity<Guid>
{
    public int UserId { get; set; }
    public required string Token { get; set; }
    public DateTime Expiration { get; set; }
    public bool IsRevoked { get; set; }

    public User User { get; set; }
    
}