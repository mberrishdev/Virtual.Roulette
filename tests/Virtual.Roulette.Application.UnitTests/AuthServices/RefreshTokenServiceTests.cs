using System.Linq.Expressions;
using Common.Repository.Repository;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Options;
using Moq;
using Virtual.Roulette.Application.Exceptions;
using Virtual.Roulette.Application.Services.AuthServices;
using Virtual.Roulette.Application.Settings;
using Virtual.Roulette.Domain.Entities.RefreshTokens;

namespace Virtual.Roulette.Application.UnitTests.AuthServices;

public class RefreshTokenServiceTests
{
    private readonly Mock<IRepository<RefreshToken>> _repoMock = new();
    private readonly Mock<IQueryRepository<RefreshToken>> _queryMock = new();
    private readonly RefreshTokenService _service;

    public RefreshTokenServiceTests()
    {
        var options = Options.Create(new AuthSettings
        {
            RefreshTokenExpirationMinutes = 5
        });

        _service = new RefreshTokenService(_repoMock.Object, _queryMock.Object, options);
    }

    [Fact]
    public async Task GenerateAndStoreRefreshTokenAsync_WhenCalled_ShouldReturnTokenAndSaveEntity()
    {
        // Arrange
        const int userId = 1;

        // Act
        var result = await _service.GenerateAndStoreRefreshTokenAsync(userId, CancellationToken.None);

        // Assert
        result.Should().NotBeNullOrWhiteSpace();

        _repoMock.Verify(r => r.InsertAsync(It.Is<RefreshToken>(t =>
            t.UserId == userId &&
            t.Token == result &&
            t.IsRevoked == false &&
            t.Expiration > DateTime.Now
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task IsValidAsync_WhenTokenIsValid_ShouldReturnTrue()
    {
        // Arrange
        var token = "valid-token";
        var refreshToken = new RefreshToken
        {
            Token = token,
            Expiration = DateTime.Now.AddMinutes(5),
            IsRevoked = false
        };

        _queryMock.Setup(x =>
                x.GetAsync(
                    It.IsAny<Expression<Func<RefreshToken, bool>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(refreshToken);

        // Act
        var result = await _service.IsValidAsync(token, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsValidAsync_WhenTokenIsExpired_ShouldReturnFalse()
    {
        // Arrange
        const string token = "expired-token";
        var expiredToken = new RefreshToken
        {
            Token = token,
            Expiration = DateTime.Now.AddMinutes(-5),
            IsRevoked = false
        };

        _queryMock.Setup(x =>
                x.GetAsync(
                    It.IsAny<Expression<Func<RefreshToken, bool>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(expiredToken);

        // Act
        var result = await _service.IsValidAsync(token, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task InvalidateAsync_WhenTokenExists_ShouldRevokeAndUpdate()
    {
        // Arrange
        const string token = "to-invalidate";
        var refreshToken = new RefreshToken { Token = token, IsRevoked = false };

        _repoMock.Setup(x =>
                x.GetForUpdateAsync(
                    It.IsAny<Expression<Func<RefreshToken, bool>>>(),
                    null,
                    It.IsAny<CancellationToken>()))!
            .ReturnsAsync(refreshToken);

        // Act
        await _service.InvalidateAsync(token, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            refreshToken.IsRevoked.Should().BeTrue();
            _repoMock.Verify(r => r.UpdateAsync(refreshToken, CancellationToken.None), Times.Once);
        }
    }

    [Fact]
    public async Task InvalidateAsync_WhenTokenNotFound_ShouldThrowException()
    {
        // Arrange
        const string token = "missing";

        _repoMock.Setup(x =>
                x.GetForUpdateAsync(
                    It.IsAny<Expression<Func<RefreshToken, bool>>>(),
                    null,
                    It.IsAny<CancellationToken>()))!
            .ReturnsAsync((RefreshToken?)null);

        // Act
        var act = async () => await _service.InvalidateAsync(token, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ObjectNotFoundException>()
            .WithMessage("*RefreshToken*Token*missing*");
    }

    [Fact]
    public async Task GetUserIdFromTokenAsync_WhenTokenExists_ShouldReturnUserId()
    {
        // Arrange
        const string token = "valid";
        var refreshToken = new RefreshToken { Token = token, UserId = 10 };

        _queryMock.Setup(x =>
                x.GetAsync(
                    It.IsAny<Expression<Func<RefreshToken, bool>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(refreshToken);

        // Act
        var result = await _service.GetUserIdFromTokenAsync(token, CancellationToken.None);

        // Assert
        result.Should().Be(10);
    }

    [Fact]
    public async Task GetUserIdFromTokenAsync_WhenTokenNotFound_ShouldReturnNull()
    {
        // Arrange
        const string token = "none";
        _queryMock.Setup(x =>
                x.GetAsync(
                    It.IsAny<Expression<Func<RefreshToken, bool>>>(),
                    null,
                    It.IsAny<CancellationToken>()))!
            .ReturnsAsync((RefreshToken?)null);

        // Act
        var result = await _service.GetUserIdFromTokenAsync(token, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}