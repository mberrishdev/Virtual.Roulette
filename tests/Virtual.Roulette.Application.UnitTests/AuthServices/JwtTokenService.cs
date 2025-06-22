using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Options;
using Virtual.Roulette.Application.Services.AuthServices;
using Virtual.Roulette.Application.Settings;
using Virtual.Roulette.Domain.Entities.Users;

namespace Virtual.Roulette.Application.UnitTests.AuthServices;

public class JwtTokenServiceTests
{
    private readonly JwtTokenService _service;

    private readonly AuthSettings _authSettings = new()
    {
        SecretKey = "super_secret_test_key_1234567890",
        TokenExpirationMinutes = 3,
        ValidIssuer = "TestIssuer",
        ValidAudience = "TestAudience"
    };

    public JwtTokenServiceTests()
    {
        var options = Options.Create(_authSettings);
        _service = new JwtTokenService(options);
    }

    [Fact]
    public void GenerateToken_WhenCalled_ShouldReturnValidJwtTokenResult()
    {
        // Arrange
        var user = new User("testuser", "hashedPassword");
        user.SetPrivateProperty(nameof(user.Id), 123);

        // Act
        var result = _service.GenerateToken(user);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrWhiteSpace();
            result.ExpiresAt.Should().BeCloseTo(DateTime.Now.AddMinutes(_authSettings.TokenExpirationMinutes),
                TimeSpan.FromSeconds(5));

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result.Token);

            token.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "123");
            token.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "testuser");

            token.Issuer.Should().Be(_authSettings.ValidIssuer);
            token.Audiences.Should().Contain(_authSettings.ValidAudience);
            token.ValidTo.ToUniversalTime().Should().BeCloseTo(result.ExpiresAt.ToUniversalTime(), TimeSpan.FromSeconds(5));
        }
    }
}