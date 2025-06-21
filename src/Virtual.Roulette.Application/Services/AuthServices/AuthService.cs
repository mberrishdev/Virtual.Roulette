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
    IOptions<AuthSettings> authSettings) : IAuthService
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

        var claims = new Claim[]
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Value.SecretKey));
        var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var expiration = DateTime.Now.AddMinutes(60);

        var jwt = new JwtSecurityToken(
            issuer: authSettings.Value.ValidIssuer,
            audience: authSettings.Value.ValidAudience,
            notBefore: DateTime.Now,
            claims: claims,
            expires: expiration,
            signingCredentials: signInCred);

        return new LoginResponse
        {
            UserId = user.Id,
            UserName = user.Username,
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            TokenExpiration = expiration,
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