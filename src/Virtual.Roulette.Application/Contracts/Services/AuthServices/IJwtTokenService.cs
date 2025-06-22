using Virtual.Roulette.Application.Contracts.Services.AuthServices.Models;
using Virtual.Roulette.Domain.Entities.Users;

namespace Virtual.Roulette.Application.Contracts.Services.AuthServices;

public interface IJwtTokenService
{
    JwtTokenResult GenerateToken(User user);
}