using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Virtual.Roulette.Application.Contracts.Services.AccountServices;
using Virtual.Roulette.Application.Contracts.Services.AuthServices;
using Virtual.Roulette.Application.Contracts.Services.BetServices;
using Virtual.Roulette.Application.Contracts.Services.JackpotServices;
using Virtual.Roulette.Application.Contracts.Services.SpinServices;
using Virtual.Roulette.Application.Services.AccountServices;
using Virtual.Roulette.Application.Services.AuthServices;
using Virtual.Roulette.Application.Services.BetServices;
using Virtual.Roulette.Application.Services.JackpotServices;
using Virtual.Roulette.Application.Services.SpinsServices;
using Virtual.Roulette.Application.Settings;

namespace Virtual.Roulette.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ISpinService, SpinService>();
        services.AddScoped<IBetService, BetService>();
        
        services.AddSingleton<IJackpotService, JackpotService>();

        return services;
    }
}