namespace Virtual.Roulette.Application.Contracts.Services.AuthServices;

public interface IRefreshTokenService
{
    Task<string> GenerateAndStoreRefreshTokenAsync(int userId, CancellationToken cancellationToken);
    Task<bool> IsValidAsync(string token, CancellationToken cancellationToken);
    Task InvalidateAsync(string token, CancellationToken cancellationToken);
    Task<int?> GetUserIdFromTokenAsync(string token, CancellationToken cancellationToken);
}