using System.ComponentModel.DataAnnotations;
using Virtual.Roulette.Domain.Entities.Accounts;
using Virtual.Roulette.Domain.Entities.Spins;
using Virtual.Roulette.Domain.Primitives;

namespace Virtual.Roulette.Domain.Entities.Users;

public class User : Entity<int>
{
    [Required, MaxLength(50)] public string Username { get; private set; }

    [Required, MaxLength(255)] public string PasswordHash { get; private set; }

    [Required] public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Account Account { get; private set; }
    public List<Spin> Spins { get; private set; }

    // Optional constructor for EF Core
    private User()
    {
    }

    public User(string username, string passwordHash)
    {
        Username = username;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        Account = new Account();
    }
}