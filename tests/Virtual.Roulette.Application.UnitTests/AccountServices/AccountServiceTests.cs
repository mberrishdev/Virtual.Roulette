using Common.Repository.Repository;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Virtual.Roulette.Application.Exceptions;
using Virtual.Roulette.Application.Services.AccountServices;
using Virtual.Roulette.Domain.Entities.Accounts;

namespace Virtual.Roulette.Application.UnitTests.AccountServices;

public class AccountServiceTests
{
    private readonly Mock<IQueryRepository<Account>> _queryRepoMock = new();
    private readonly Mock<IRepository<Account>> _repoMock = new();
    private readonly AccountService _service;

    public AccountServiceTests()
    {
        _service = new AccountService(_queryRepoMock.Object, _repoMock.Object);
    }

    [Fact]
    public async Task GetAccount_WhenAccountExists_ShouldReturnAccountModel()
    {
        // Arrange
        var account = new Account();
        account.SetPrivateProperty(nameof(account.Id), 1);
        account.SetPrivateProperty(nameof(account.UserId), 123);
        account.SetPrivateProperty(nameof(account.Balance), 50.75m);
        account.SetPrivateProperty(nameof(account.Currency), "USD");

        _queryRepoMock
            .Setup(r => r.GetAsync(a => a.UserId == 123, null, CancellationToken.None))
            .ReturnsAsync(account);

        // Act
        var result = await _service.GetAccount(123, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Id.Should().Be(1);
            result.UserId.Should().Be(123);
            result.Balance.Should().Be(50.75m);
            result.BalanceInCents.Should().Be(5075);
            result.Currency.Should().Be("USD");
        }
    }

    [Fact]
    public async Task GetAccount_WhenAccountDoesNotExist_ShouldThrowObjectNotFoundException()
    {
        // Arrange
        _queryRepoMock
            .Setup(r => r.GetAsync(a => a.UserId == 123, null, CancellationToken.None))!
            .ReturnsAsync((Account?)null);

        // Act
        Func<Task> act = async () => await _service.GetAccount(123, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<ObjectNotFoundException>()
            .WithMessage("*Account*UserId*123*");
    }

    [Fact]
    public async Task WithdrawAsync_WhenAccountExists_ShouldCallWithdrawAndUpdate()
    {
        // Arrange
        var account = new Account();
        account.SetPrivateProperty(nameof(account.Id), 1);
        account.SetPrivateProperty(nameof(account.UserId), 123);
        account.SetPrivateProperty(nameof(account.Balance), 100m);
        account.SetPrivateProperty(nameof(account.Currency), "USD");

        _repoMock
            .Setup(r => r.GetForUpdateAsync(a => a.UserId == 123, null, CancellationToken.None))
            .ReturnsAsync(account);

        // Act
        await _service.WithdrawAsync(123, 20m, CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.UpdateAsync(account, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DepositAsync_WhenAccountExists_ShouldCallDepositAndUpdate()
    {
        // Arrange
        var account = new Account();
        account.SetPrivateProperty(nameof(account.Id), 1);
        account.SetPrivateProperty(nameof(account.UserId), 123);
        account.SetPrivateProperty(nameof(account.Balance), 80m);
        account.SetPrivateProperty(nameof(account.Currency), "USD");

        _repoMock
            .Setup(r => r.GetForUpdateAsync(a => a.UserId == 123, null, CancellationToken.None))
            .ReturnsAsync(account);

        // Act
        await _service.DepositAsync(123, 50m, CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.UpdateAsync(account, CancellationToken.None), Times.Once);
    }
}
