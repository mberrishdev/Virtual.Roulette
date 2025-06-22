using Microsoft.EntityFrameworkCore;
using Virtual.Roulette.Domain.Entities.Accounts;
using Virtual.Roulette.Domain.Entities.RefreshTokens;
using Virtual.Roulette.Domain.Entities.Spins;
using Virtual.Roulette.Domain.Entities.Users;

namespace Virtual.Roulette.Persistence.Database;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Spin> Spins { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}