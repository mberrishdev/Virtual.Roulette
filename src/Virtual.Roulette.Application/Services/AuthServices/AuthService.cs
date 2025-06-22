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
using Virtual.Roulette.Application.Models;
using Virtual.Roulette.Application.Settings;
using Virtual.Roulette.Domain.Entities.Users;

namespace Virtual.Roulette.Application.Services.AuthServices;

public class AuthService(
    IQueryRepository<User> userQueryRepository,
    IRepository<User> useRepository,
    IOptions<AuthSettings> authSettings,
    IRefreshTokenService refreshTokenService) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(AuthRequest authRequest, CancellationToken cancellationToken)
    {
        var user = await userQueryRepository.GetAsync(u => u.Username == authRequest.UserName,
            cancellationToken: cancellationToken);

        const string errorMessage = "Invalid credentials.";

        if (!HashHelper.Hash(authRequest.Password).Equals(user.PasswordHash))
        {
            throw new InvalidCredentialsException(errorMessage);
        }

        var expiration = DateTime.Now.AddMinutes(60);

        var jwtToken = GenerateJwtToken(user);

        var refreshToken = await refreshTokenService.GenerateAndStoreRefreshTokenAsync(user.Id, cancellationToken);

        return new LoginResponse
        {
            UserId = user.Id,
            UserName = user.Username,
            Token = jwtToken,
            TokenExpiration = expiration,
            RefreshToken = refreshToken,
        };
    }

    public async Task<LoginResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var isValid = await refreshTokenService.IsValidAsync(refreshToken, cancellationToken);
        if (!isValid) throw new UnauthorizedAccessException("Refresh token is invalid or expired.");

        var userId = await refreshTokenService.GetUserIdFromTokenAsync(refreshToken, cancellationToken);
        if (userId is null) throw new UnauthorizedAccessException("User not found.");

        await refreshTokenService.InvalidateAsync(refreshToken, cancellationToken);

        var user = await userQueryRepository.GetAsync(u => u.Id == userId, cancellationToken: cancellationToken);

        var newAccessToken = GenerateJwtToken(user);
        var newRefreshToken = await refreshTokenService.GenerateAndStoreRefreshTokenAsync(user.Id, cancellationToken);

        return new LoginResponse
        {
            UserId = user.Id,
            UserName = user.Username,
            Token = newAccessToken,
            TokenExpiration = DateTime.UtcNow.AddMinutes(authSettings.Value.TokenExpirationMinutes),
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

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Value.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: authSettings.Value.ValidIssuer,
            audience: authSettings.Value.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(authSettings.Value.TokenExpirationMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}