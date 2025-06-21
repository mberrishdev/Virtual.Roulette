using Virtual.Roulette.Domain.Entities.Spins;

namespace Virtual.Roulette.Application.Contracts.Services.SpinServices;

public interface ISpinService
{
    Task<Guid> CreateSpinAsync(Spin spin, CancellationToken cancellationToken);
    Task<List<Spin>> GetSpinsByUserAsync(int userId, CancellationToken cancellationToken);
}