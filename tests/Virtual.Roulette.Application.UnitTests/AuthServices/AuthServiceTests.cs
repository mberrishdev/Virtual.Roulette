using System.Linq.Expressions;
using Common.Repository.Repository;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Moq;
using Virtual.Roulette.Application.Contracts.Services.AuthService.Models;
using Virtual.Roulette.Application.Contracts.Services.AuthServices;
using Virtual.Roulette.Application.Contracts.Services.AuthServices.Models;
using Virtual.Roulette.Application.Exceptions;
using Virtual.Roulette.Application.Helpers;
using Virtual.Roulette.Application.Services.AuthServices;
using Virtual.Roulette.Application.Settings;
using Virtual.Roulette.Domain.Entities.Users;

namespace Virtual.Roulette.Application.UnitTests.AuthServices;

public class AuthServiceTests
{
    private readonly Mock<IQueryRepository<User>> _userQueryRepoMock = new();
    private readonly Mock<IRepository<User>> _userRepoMock = new();
    private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock = new();
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock = new();
    private readonly AuthService _authService;
    
    public AuthServiceTests()
    {
        _authService = new AuthService(
            _userQueryRepoMock.Object,
            _userRepoMock.Object,
            _refreshTokenServiceMock.Object,
            _jwtTokenServiceMock.Object);
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ShouldReturnAuthResponse()
    {
        // Arrange
        var user = new User("john", HashHelper.Hash("1234"));
        user.SetPrivateProperty(nameof(User.Id), 101);

        var request = new AuthRequest { UserName = "john", Password = "1234" };

        _userQueryRepoMock.Setup(x =>
                x.GetAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);


        _jwtTokenServiceMock.Setup(x => x.GenerateToken(user))
            .Returns(new JwtTokenResult("jwt.token", DateTime.UtcNow.AddMinutes(3)));

        _refreshTokenServiceMock.Setup(x => x.GenerateAndStoreRefreshTokenAsync(101, It.IsAny<CancellationToken>()))
            .ReturnsAsync("refresh-token");

        // Act
        var result = await _authService.LoginAsync(request, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.UserId.Should().Be(101);
            result.UserName.Should().Be("john");
            result.Token.Should().Be("jwt.token");
            result.RefreshToken.Should().Be("refresh-token");
        }
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsInvalid_ShouldThrowInvalidCredentials()
    {
        // Arrange
        var user = new User("john", HashHelper.Hash("correct"));
        user.SetPrivateProperty(nameof(User.Id), 1);

        var request = new AuthRequest { UserName = "john", Password = "wrong" };

        _userQueryRepoMock.Setup(x =>
                x.GetAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _authService.LoginAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>()
            .WithMessage("Invalid credentials.");
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ShouldThrowInvalidCredentials()
    {
        // Arrange
        var request = new AuthRequest { UserName = "john", Password = "wrong" };

        _userQueryRepoMock.Setup(x =>
                x.GetAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        // Act
        Func<Task> act = async () => await _authService.LoginAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>()
            .WithMessage("Invalid credentials.");
    }

    [Fact]
    public async Task RefreshAsync_WhenTokenIsValid_ShouldReturnNewToken()
    {
        // Arrange
        var user = new User("john", "hash");
        user.SetPrivateProperty(nameof(User.Id), 123);

        _refreshTokenServiceMock.Setup(x => x.IsValidAsync("refresh-token", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _refreshTokenServiceMock.Setup(x => x.GetUserIdFromTokenAsync("refresh-token", It.IsAny<CancellationToken>()))
            .ReturnsAsync(123);

        _refreshTokenServiceMock.Setup(x => x.InvalidateAsync("refresh-token", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _userQueryRepoMock.Setup(x =>
                x.GetAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _jwtTokenServiceMock.Setup(x => x.GenerateToken(user))
            .Returns(new JwtTokenResult("new.jwt.token", DateTime.UtcNow.AddMinutes(3)));

        _refreshTokenServiceMock.Setup(x =>
                x.GenerateAndStoreRefreshTokenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("new-refresh-token");

        // Act
        var result = await _authService.RefreshAsync("refresh-token", CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.UserId.Should().Be(123);
            result.UserName.Should().Be("john");
            result.Token.Should().Be("new.jwt.token");
            result.RefreshToken.Should().Be("new-refresh-token");
        }
    }
}