using Common.Repository.Repository;
using FluentAssertions;
using Moq;
using Virtual.Roulette.Application.Hubs;
using Virtual.Roulette.Application.Services.JackpotServices;
using Virtual.Roulette.Domain.Entities.Jackpots;
using Microsoft.AspNetCore.SignalR;

namespace Virtual.Roulette.Application.UnitTests.JackpotServices;

public class JackpotServiceTests
{
    private readonly Mock<IRepository<Jackpot>> _repoMock = new();
    private readonly Mock<IQueryRepository<Jackpot>> _queryRepoMock = new();
    private readonly Mock<IHubContext<JackpotHub, IJackpotClient>> _hubContextMock = new();
    private readonly Mock<IHubClients<IJackpotClient>> _clientsMock = new();
    private readonly Mock<IJackpotClient> _jackpotClientMock = new();

    private readonly JackpotService _sut;

    public JackpotServiceTests()
    {
        _hubContextMock.Setup(h => h.Clients).Returns(_clientsMock.Object);
        _clientsMock.Setup(c => c.All).Returns(_jackpotClientMock.Object);

        _sut = new JackpotService(_repoMock.Object, _queryRepoMock.Object, _hubContextMock.Object);
    }

    [Fact]
    public async Task AddToJackpot_WhenCalled_ShouldInsert1PercentIntoDb()
    {
        // Arrange
        var bet = 1000m;
        var expected = bet * 0.01m;

        // Act
        await _sut.AddToJackpot(bet, CancellationToken.None);

        // Assert
        _repoMock.Verify(r =>
                r.InsertAsync(It.Is<Jackpot>(j => j.Amount == expected),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetJackpotAmountAsync_WhenCalled_ShouldReturnSumOfAllJackpots()
    {
        // Arrange
        var jackpots = new List<Jackpot>
        {
            new(10),
            new(20),
            new(30)
        };
        _queryRepoMock.Setup(q => q.GetListAsync(null, null, null, null, null, CancellationToken.None))
            .ReturnsAsync(jackpots);

        // Act
        var result = await _sut.GetJackpotAmountAsync(default);

        // Assert
        result.Should().Be(60);
    }

    [Fact]
    public async Task BroadcastJackpotUpdateAsync_WhenCalled_ShouldBroadcastCurrentAmount()
    {
        // Arrange
        var jackpots = new List<Jackpot> { new(100), new(200) };
        _queryRepoMock.Setup(q => q.GetListAsync(null, null, null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(jackpots);

        // Act
        await _sut.BroadcastJackpotUpdateAsync(CancellationToken.None);

        // Assert
        _jackpotClientMock.Verify(c => c.JackpotUpdated(300), Times.Once);
    }
}