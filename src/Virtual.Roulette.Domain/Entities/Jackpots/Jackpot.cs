using Virtual.Roulette.Domain.Primitives;

namespace Virtual.Roulette.Domain.Entities.Jackpots;

public class Jackpot : Entity<Guid>
{
    public decimal Amount { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Jackpot()
    {
    }
    
    public Jackpot(decimal amount)
    {
        Amount = amount;
        CreatedAt = DateTime.Now;
    }
}