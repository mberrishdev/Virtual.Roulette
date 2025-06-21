using Common.Repository.Repository;
using Virtual.Roulette.Application.Contracts.Services.SpinServices;
using Virtual.Roulette.Domain.Entities.Spins;

namespace Virtual.Roulette.Application.Services.SpinsServices;

public class SpinService(IRepository<Spin> spinRepository, IQueryRepository<Spin> spinQueryRepository) : ISpinService
{
    public async Task<Guid> CreateSpinAsync(Spin spin, CancellationToken cancellationToken)
    {
        spin.CreatedAt = DateTime.Now;
        await spinRepository.InsertAsync(spin, cancellationToken);
        
        return spin.Id;
    }

    public async Task<List<Spin>> GetSpinsByUserAsync(int userId, CancellationToken cancellationToken)
    {
        var spins = await spinQueryRepository.GetListAsync(
            predicate: s => s.UserId == userId,
            cancellationToken: cancellationToken
        );

        return spins;
    }
}