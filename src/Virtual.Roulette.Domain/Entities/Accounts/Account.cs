using System.ComponentModel.DataAnnotations;
using Virtual.Roulette.Domain.Entities.Users;
using Virtual.Roulette.Domain.Primitives;

namespace Virtual.Roulette.Domain.Entities.Accounts;

public class Account : Entity<int>
{
    [Required] public int UserId { get; private set; }

    [Required] public decimal Balance { get; private set; }

    [Required, MinLength(3), MaxLength(3)] public string Currency { get; private set; } = "USD";

    [Required] public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public User User { get; private set; }

    public Account()
    {
        Balance = 100;
        CreatedAt = DateTime.Now;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

        if (Balance < amount)
            throw new InvalidOperationException("Insufficient balance.");

        Balance -= amount;
        UpdatedAt = DateTime.Now;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

        Balance += amount;
        UpdatedAt = DateTime.Now;
    }
}