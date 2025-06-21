using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Virtual.Roulette.Persistence.Database;
using HubDocs;
using Virtual.Roulette.Application.Hubs;

namespace Virtual.Roulette.Api.Infrastructure.StartupConfiguration;

/// <summary>
/// Middleware configuration
/// </summary>
public static class MiddlewareConfiguration
{
    /// <summary>
    /// Configure middleware
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UsePathBase("/api");
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseDatabaseMigration<AppDbContext>();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI(o => { o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); });
        //}

        app.UseHttpsRedirection();

        app.UseRouting();

        // CORS middleware must be configured to execute between the calls to UseRouting and UseEndpoints.
        // See details here: https://docs.microsoft.com/en-us/aspnet/core/security/cors
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapHealthChecks("/health", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHubs();
        
        return app;
    }

    private static void UseDatabaseMigration<TDbContext>(this IApplicationBuilder builder) where TDbContext : DbContext
    {
        using var serviceScope = builder.ApplicationServices.CreateScope();
        var service = serviceScope.ServiceProvider?.GetService<TDbContext>();
        service?.Database.Migrate();
    }
    
    
    #region Hubs

    private static WebApplication MapHubs(this WebApplication app)
    {
        app.MapHubAndRegister<JackpotHub>("/hub/jackpot");

        app.AddHubDocs();
        return app;
    }

    #endregion
}