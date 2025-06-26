using System.Linq.Expressions;
using Common.Repository.Repository;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Virtual.Roulette.Application.Services.SpinsServices;
using Virtual.Roulette.Domain.Entities.Spins;

namespace Virtual.Roulette.Application.UnitTests.SpinsServices;

public class SpinServiceTests
{
    private readonly Mock<IRepository<Spin>> _spinRepoMock = new();
    private readonly Mock<IQueryRepository<Spin>> _spinQueryRepoMock = new();
    private readonly SpinService _spinService;

    public SpinServiceTests()
    {
        _spinService = new SpinService(_spinRepoMock.Object, _spinQueryRepoMock.Object);
    }

    [Fact]
    public async Task CreateSpinAsync_WhenCalled_ShouldInsertSpinAndReturnId()
    {
        // Arrange
        var spin = new Spin(1, "", 3, 4, 5, "");
        var spinId = Guid.NewGuid();
        spin.SetPrivateProperty(nameof(Spin.Id), spinId);

        // Act
        var result = await _spinService.CreateSpinAsync(spin, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().Be(spinId);
            _spinRepoMock.Verify(r => r.InsertAsync(spin, CancellationToken.None), Times.Once);
            spin.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        }
    }

    [Fact]
    public async Task GetSpinsByUserAsync_WhenCalled_ShouldReturnSpinsForUser()
    {
        // Arrange
        var userId = 42;
        var spins = new List<Spin>
        {
            new Spin(userId, "", 3, 4, 5, ""),
            new Spin(userId, "", 3, 4, 5, "")
        };

        _spinQueryRepoMock.Setup(x =>
                x.GetListAsync(
                    null,
                    It.IsAny<Expression<Func<Spin, bool>>>(),
                    null,
                    null,
                    null,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(spins);

        // Act
        var result = await _spinService.GetSpinsByUserAsync(userId, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().HaveCount(2);
            result.Should().OnlyContain(s => s.UserId == userId);
        }
    }
}