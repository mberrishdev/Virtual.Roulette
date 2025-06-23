using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Virtual.Roulette.Application.Hubs;
using Virtual.Roulette.Application.Services.JackpotServices;

namespace Virtual.Roulette.Application.UnitTests.JackpotServices;

public class JackpotServiceTests
{
    private readonly Mock<IHubContext<JackpotHub, IJackpotClient>> _hubContextMock;
    private readonly Mock<IHubClients<IJackpotClient>> _clientsMock;
    private readonly Mock<IJackpotClient> _jackpotClientMock;
    private readonly JackpotService _sut;

    public JackpotServiceTests()
    {
        _hubContextMock = new Mock<IHubContext<JackpotHub, IJackpotClient>>();
        _clientsMock = new Mock<IHubClients<IJackpotClient>>();
        _jackpotClientMock = new Mock<IJackpotClient>();

        _hubContextMock.Setup(x => x.Clients).Returns(_clientsMock.Object);
        _clientsMock.Setup(x => x.All).Returns(_jackpotClientMock.Object);

        _sut = new JackpotService(_hubContextMock.Object);
    }

    [Fact]
    public void GetJackpot_WhenCalled_ShouldReturnJackpotModel()
    {
        // Act
        var result = _sut.GetJackpot();

        // Assert
        result.Should().NotBeNull();
        result.Amount.Should().Be(0);
    }

    [Fact]
    public void GetJackpot_WhenAmountIsSet_ShouldReturnCorrectAmount()
    {
        // Arrange
        var expectedAmount = 1000m;
        _sut.SetAmount(expectedAmount);

        // Act
        var result = _sut.GetJackpot();

        // Assert
        result.Should().NotBeNull();
        result.Amount.Should().Be(expectedAmount);
    }

    [Theory]
    [InlineData(100, 1)]
    [InlineData(500, 5)]
    [InlineData(1000, 10)]
    [InlineData(0, 0)]
    public void AddToJackpot_WhenBetAmountProvided_ShouldAdd1PercentToJackpot(decimal betAmount,
        decimal expectedAddition)
    {
        // Arrange
        var initialAmount = 100m;
        _sut.SetAmount(initialAmount);

        // Act
        var newAmount = _sut.AddToJackpot(betAmount);

        // Assert
        newAmount.Should().Be(initialAmount + expectedAddition);
        _sut.GetJackpot().Amount.Should().Be(initialAmount + expectedAddition);
    }

    [Fact]
    public void AddToJackpot_WhenCalledMultipleTimes_ShouldAccumulateCorrectly()
    {
        // Arrange
        const decimal firstBet = 100m;
        const decimal secondBet = 200m;
        var expectedTotal = (firstBet * 0.01m) + (secondBet * 0.01m);

        // Act
        _sut.AddToJackpot(firstBet);
        var finalAmount = _sut.AddToJackpot(secondBet);

        // Assert
        finalAmount.Should().Be(expectedTotal);
        _sut.GetJackpot().Amount.Should().Be(expectedTotal);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(1000.50)]
    [InlineData(-100)]
    public void SetAmount_WhenAmountProvided_ShouldSetCorrectAmount(decimal amount)
    {
        // Act
        _sut.SetAmount(amount);

        // Assert
        _sut.GetJackpot().Amount.Should().Be(amount);
    }

    [Fact]
    public void SetAmount_WhenCalledAfterAddToJackpot_ShouldOverrideAmount()
    {
        // Arrange
        _sut.AddToJackpot(1000m);
        var newAmount = 500m;

        // Act
        _sut.SetAmount(newAmount);

        // Assert
        _sut.GetJackpot().Amount.Should().Be(newAmount);
    }

    [Fact]
    public async Task BroadcastJackpotUpdateAsync_WhenCalled_ShouldCallJackpotUpdatedOnAllClients()
    {
        // Arrange
        const decimal jackpotAmount = 1500m;
        _sut.SetAmount(jackpotAmount);

        // Act
        await _sut.BroadcastJackpotUpdateAsync();

        // Assert
        _jackpotClientMock.Verify(x => x.JackpotUpdated(jackpotAmount), Times.Once);
    }

    [Fact]
    public async Task BroadcastJackpotUpdateAsync_WhenCalledWithZeroAmount_ShouldBroadcastZero()
    {
        // Act
        await _sut.BroadcastJackpotUpdateAsync();

        // Assert
        _jackpotClientMock.Verify(x => x.JackpotUpdated(0), Times.Once);
    }

    [Fact]
    public async Task BroadcastJackpotUpdateAsync_WhenJackpotAmountChanges_ShouldBroadcastCurrentAmount()
    {
        // Arrange
        _sut.AddToJackpot(1000m);
        var expectedAmount = _sut.GetJackpot().Amount;

        // Act
        await _sut.BroadcastJackpotUpdateAsync();

        // Assert
        _jackpotClientMock.Verify(x => x.JackpotUpdated(expectedAmount), Times.Once);
    }
}