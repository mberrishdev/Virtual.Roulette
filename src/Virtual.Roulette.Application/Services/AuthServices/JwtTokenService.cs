using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Virtual.Roulette.Application.Contracts.Services.AuthServices;
using Virtual.Roulette.Application.Contracts.Services.AuthServices.Models;
using Virtual.Roulette.Application.Settings;
using Virtual.Roulette.Domain.Entities.Users;

namespace Virtual.Roulette.Application.Services.AuthServices;

public class JwtTokenService(IOptions<AuthSettings> authSettings) : IJwtTokenService
{
    public JwtTokenResult GenerateToken(User user)
    {
        var expiration = DateTime.Now.AddMinutes(authSettings.Value.TokenExpirationMinutes);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        };

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Value.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: authSettings.Value.ValidIssuer,
            audience: authSettings.Value.ValidAudience,
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

        return new JwtTokenResult(new JwtSecurityTokenHandler().WriteToken(token), expiration);
    }
}