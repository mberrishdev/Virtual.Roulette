using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Repository.Repository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Virtual.Roulette.Application.Contracts.Services.AuthService.Models;
using Virtual.Roulette.Application.Contracts.Services.AuthServices;
using Virtual.Roulette.Application.Contracts.Services.AuthServices.Models;
using Virtual.Roulette.Application.Exceptions;
using Virtual.Roulette.Application.Helpers;
using Virtual.Roulette.Application.Settings;
using Virtual.Roulette.Domain.Entities.Users;

namespace Virtual.Roulette.Application.Services.AuthServices;

public class AuthService(
    IQueryRepository<User> userQueryRepository,
    IRepository<User> useRepository,
    IRefreshTokenService refreshTokenService,
    IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<AuthResponse> LoginAsync(AuthRequest authRequest, CancellationToken cancellationToken)
    {
        var user = await userQueryRepository.GetAsync(u => u.Username == authRequest.UserName,
            cancellationToken: cancellationToken);

        const string errorMessage = "Invalid credentials.";

        if (user == null || !HashHelper.Hash(authRequest.Password).Equals(user.PasswordHash))
        {
            throw new InvalidCredentialsException(errorMessage);
        }

        var tokenResult = jwtTokenService.GenerateToken(user);

        var refreshToken = await refreshTokenService.GenerateAndStoreRefreshTokenAsync(user.Id, cancellationToken);

        return new AuthResponse
        {
            UserId = user.Id,
            UserName = user.Username,
            Token = tokenResult.Token,
            TokenExpiration = tokenResult.ExpiresAt,
            RefreshToken = refreshToken,
        };
    }

    public async Task<AuthResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var isValid = await refreshTokenService.IsValidAsync(refreshToken, cancellationToken);
        if (!isValid) throw new UnauthorizedAccessException("Refresh token is invalid or expired.");

        var userId = await refreshTokenService.GetUserIdFromTokenAsync(refreshToken, cancellationToken);
        if (userId is null) throw new UnauthorizedAccessException("User not found.");

        await refreshTokenService.InvalidateAsync(refreshToken, cancellationToken);

        var user = await userQueryRepository.GetAsync(u => u.Id == userId, cancellationToken: cancellationToken);

        var tokenResult = jwtTokenService.GenerateToken(user);

        var newRefreshToken = await refreshTokenService.GenerateAndStoreRefreshTokenAsync(user.Id, cancellationToken);

        return new AuthResponse
        {
            UserId = user.Id,
            UserName = user.Username,
            Token = tokenResult.Token,
            TokenExpiration = tokenResult.ExpiresAt,
            RefreshToken = newRefreshToken
        };
    }

    public async Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var existingUser = await userQueryRepository.GetAsync(u => u.Username == request.Username,
            cancellationToken: cancellationToken);
        if (existingUser != null)
        {
            throw new ObjectAlreadyExistException(nameof(User), nameof(request.Username), request.Username);
        }

        var user = new User(request.Username, HashHelper.Hash(request.Password));

        await useRepository.InsertAsync(user, cancellationToken);
    }
}