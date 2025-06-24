using Virtual.Roulette.Application.Contracts.Services.SpinServices.Models;
using Virtual.Roulette.Domain.Entities.Spins;

namespace Virtual.Roulette.Application.Contracts.Services.SpinServices;

public interface ISpinService
{
    Task<Guid> CreateSpinAsync(Spin spin, CancellationToken cancellationToken);
    Task<List<SpinModel>> GetSpinsByUserAsync(int userId, CancellationToken cancellationToken);
}