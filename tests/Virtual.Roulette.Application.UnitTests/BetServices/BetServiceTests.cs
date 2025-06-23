using Common.Repository.UnitOfWork;
using FluentAssertions;
using FluentAssertions.Execution;
using ge.singular.roulette;
using Moq;
using Virtual.Roulette.Application.Contracts.Services.AccountServices;
using Virtual.Roulette.Application.Contracts.Services.AccountServices.Models;
using Virtual.Roulette.Application.Contracts.Services.BetServices;
using Virtual.Roulette.Application.Contracts.Services.JackpotServices;
using Virtual.Roulette.Application.Contracts.Services.SpinServices;
using Virtual.Roulette.Application.Services.BetServices;
using Virtual.Roulette.Domain.Entities.Spins;

namespace Virtual.Roulette.Application.UnitTests.BetServices;

public class BetServiceTests
{
    private readonly Mock<ISpinService> _spinServiceMock = new();
    private readonly Mock<IAccountService> _accountServiceMock = new();
    private readonly Mock<IJackpotService> _jackpotServiceMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUnitOfWorkScope> _unitOfWorkScopeMock = new();
    private readonly Mock<IBetValidator> _betValidatorMock = new();

    private readonly BetService _betService;

    public BetServiceTests()
    {
        _unitOfWorkMock.Setup(u => u.CreateScopeAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_unitOfWorkScopeMock.Object);

        _betService = new BetService(
            _spinServiceMock.Object,
            _accountServiceMock.Object,
            _jackpotServiceMock.Object,
            _betValidatorMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task PlaceBetAsync_WhenBetIsInvalid_ShouldThrowException()
    {
        // Arrange
        const int userId = 1;
        const string betJson = "invalid-bet";

        var response = new IsBetValidResponse();
        response.setIsValid(false);

        _betValidatorMock.Setup(x => x.IsValid(betJson)).Returns(response);

        // Act
        Func<Task> act = async () => await _betService.PlaceBetAsync(userId, betJson, "127.0.0.1", CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Bet is invalid");
    }
    
    [Fact]
    public async Task PlaceBetAsync_WhenValidAndUserWins_ShouldReturnExpectedResult()
    {
        // Arrange
        const int userId = 1;
        const string betJson = "valid-bet";
        const string ip = "127.0.0.1";
        const int winningNumber = 5;

        var account = new AccountModel
        {
            Id = 10,
            UserId = userId,
            Balance = 10.00m,
            Currency = "USD"
        };

        _accountServiceMock.Setup(a => a.GetAccount(userId, CancellationToken.None)).ReturnsAsync(account);
        _spinServiceMock.Setup(s => s.CreateSpinAsync(It.IsAny<Spin>(), CancellationToken.None))
            .ReturnsAsync(Guid.NewGuid());

        const int betAmount = 5;
        const int winAmount = 1500;

        var betValidResponse = new IsBetValidResponse();
        betValidResponse.setIsValid(true);
        betValidResponse.setBetAmount(5);
        
        _betValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(betValidResponse);
        
        _betValidatorMock.Setup(x => x.EstimateWin(It.IsAny<string>(), It.IsAny<int>())).Returns(winAmount);

        // Act
        var result = await _betService.PlaceBetAsync(userId, betJson, ip, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.WinningNumber.Should().BeGreaterThanOrEqualTo(0).And.BeLessThanOrEqualTo(36);
            result.WonAmount.Should().Be(winAmount);
        }

        _accountServiceMock.Verify(a => a.WithdrawAsync(userId, betAmount, CancellationToken.None), Times.Once);
        _accountServiceMock.Verify(a => a.DepositAsync(userId, winAmount / 100.0m, CancellationToken.None), Times.Once);
        _jackpotServiceMock.Verify(j => j.AddToJackpot(winAmount / 100.0m), Times.Once);
        _jackpotServiceMock.Verify(j => j.BroadcastJackpotUpdateAsync(), Times.Once);
    }
        
    [Fact]
    public async Task PlaceBetAsync_WhenWinAmountIs0_ShouldNotDeposit()
    {
        // Arrange
        const int userId = 1;
        const string betJson = "valid-bet";
        const string ip = "127.0.0.1";
        const int winningNumber = 5;

        var account = new AccountModel
        {
            Id = 10,
            UserId = userId,
            Balance = 10.00m,
            Currency = "USD"
        };

        _accountServiceMock.Setup(a => a.GetAccount(userId, CancellationToken.None)).ReturnsAsync(account);
        _spinServiceMock.Setup(s => s.CreateSpinAsync(It.IsAny<Spin>(), CancellationToken.None))
            .ReturnsAsync(Guid.NewGuid());

        const int betAmount = 5;
        const int winAmount = 0;

        var betValidResponse = new IsBetValidResponse();
        betValidResponse.setIsValid(true);
        betValidResponse.setBetAmount(5);
        
        _betValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(betValidResponse);
        
        _betValidatorMock.Setup(x => x.EstimateWin(It.IsAny<string>(), It.IsAny<int>())).Returns(winAmount);

        // Act
        var result = await _betService.PlaceBetAsync(userId, betJson, ip, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.WinningNumber.Should().BeGreaterThanOrEqualTo(0).And.BeLessThanOrEqualTo(36);
            result.WonAmount.Should().Be(winAmount);
        }

        _accountServiceMock.Verify(a => a.WithdrawAsync(userId, betAmount, CancellationToken.None), Times.Once);
        _accountServiceMock.Verify(a => a.DepositAsync(userId, winAmount / 100.0m, CancellationToken.None), Times.Never);
        _jackpotServiceMock.Verify(j => j.AddToJackpot(winAmount / 100.0m), Times.Once);
        _jackpotServiceMock.Verify(j => j.BroadcastJackpotUpdateAsync(), Times.Once);
    }
}