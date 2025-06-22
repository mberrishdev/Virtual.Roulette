using Virtual.Roulette.Application.Contracts.Services.AuthService.Models;
using Virtual.Roulette.Application.Contracts.Services.AuthServices.Models;
using Virtual.Roulette.Application.Models;

namespace Virtual.Roulette.Application.Contracts.Services.AuthServices;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(AuthRequest authRequest, CancellationToken cancellationToken);
    Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<LoginResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
}