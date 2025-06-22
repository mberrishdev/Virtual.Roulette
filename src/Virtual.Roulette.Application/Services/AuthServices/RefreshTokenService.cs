using System.Security.Cryptography;
using Common.Repository.Repository;
using Microsoft.Extensions.Options;
using Virtual.Roulette.Application.Contracts.Services.AuthServices;
using Virtual.Roulette.Application.Exceptions;
using Virtual.Roulette.Application.Settings;
using Virtual.Roulette.Domain.Entities.RefreshTokens;

namespace Virtual.Roulette.Application.Services.AuthServices;

public class RefreshTokenService(
    IRepository<RefreshToken> refreshTokenRepository,
    IQueryRepository<RefreshToken> refreshTokenQueryRepository,
    IOptions<AuthSettings> authSettings)
    : IRefreshTokenService
{
    public async Task<string> GenerateAndStoreRefreshTokenAsync(int userId, CancellationToken cancellationToken)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var entity = new RefreshToken
        {
            UserId = userId,
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(authSettings.Value.RefreshTokenExpirationMinutes),
            IsRevoked = false
        };

        await refreshTokenRepository.InsertAsync(entity, cancellationToken);
        return token;
    }

    public async Task<bool> IsValidAsync(string token, CancellationToken cancellationToken)
    {
        var stored =
            await refreshTokenQueryRepository.GetAsync(x => x.Token == token, cancellationToken: cancellationToken);
        return stored is { IsRevoked: false } && stored.Expiration > DateTime.UtcNow;
    }

    public async Task InvalidateAsync(string token, CancellationToken cancellationToken)
    {
        var stored =
            await refreshTokenRepository.GetForUpdateAsync(x => x.Token == token, cancellationToken: cancellationToken)
            ?? throw new ObjectNotFoundException(nameof(RefreshToken), nameof(RefreshToken.Token), token);

        stored.IsRevoked = true;
        await refreshTokenRepository.UpdateAsync(stored, cancellationToken);
    }

    public async Task<int?> GetUserIdFromTokenAsync(string token, CancellationToken cancellationToken)
    {
        var stored =
            await refreshTokenQueryRepository.GetAsync(x => x.Token == token, cancellationToken: cancellationToken);
        return stored?.UserId ?? null;
    }
}