namespace Virtual.Roulette.Infrastructure;

public static class DependencyInjection
{
    public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddInfrastructure(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        return services;
    }
}