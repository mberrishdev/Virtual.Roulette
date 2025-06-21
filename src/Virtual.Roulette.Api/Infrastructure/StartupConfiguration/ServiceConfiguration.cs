using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Virtual.Roulette.Api.Filters;
using Virtual.Roulette.Api.Infrastructure.Options;
using Virtual.Roulette.Application;
using Virtual.Roulette.Infrastructure;
using Virtual.Roulette.Persistence;
using Virtual.Roulette.Persistence.Database;

namespace Virtual.Roulette.Api.Infrastructure.StartupConfiguration;

public static class ServiceConfiguration
{
    public static WebApplicationBuilder ConfigureService(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        IConfiguration configuration = builder.Configuration;
        var environment = builder.Environment;

        services.AddControllers()
            .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Ref>());

        services.AddPersistence(configuration);
        services.AddApplication(configuration);
        services.AddInfrastructure(configuration);

        #region Health checks

        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>();

        #endregion

        services.AddAuthentication(configuration);

        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        services.AddVersioning();
        services.AddSwagger();

        services.AddHealthChecks();

        services.AddCors(configuration, environment);

        services.AddLogging();

        services.AddSignalR();

        return builder;
    }

    #region Authentication

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:SecretKey"] ?? "")),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["AuthSettings:ValidIssuer"],
                ValidAudience = configuration["AuthSettings:ValidAudience"],
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path.ToString();
                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWith("/hub/", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();

        return services;
    }

    #endregion

    #region Versioning

    private static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(setup =>
        {
            setup.DefaultApiVersion = new ApiVersion(1, 0);
            setup.AssumeDefaultVersionWhenUnspecified = true;
            setup.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    #endregion

    #region Swagger

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.ConfigureOptions<ConfigureSwaggerOptions>()
            .AddSwaggerGen(swagger =>
            {
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer w9ADFAqio8bjzlao10385Adjeb\"",
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // swagger.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                swagger.DocumentFilter<HealthChecksFilter>();
            });

        return services;
    }

    #endregion

    #region Cors

    private static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policyBuilder =>
                {
                    string? allowedOrigins = configuration.GetValue<string>("CorsAllowedOrigins");
                    if (string.IsNullOrEmpty(allowedOrigins) || (allowedOrigins == "*" && environment.IsDevelopment()))
                    {
                        policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    }
                    else
                    {
                        policyBuilder.WithOrigins(allowedOrigins.Split(";"))
                            .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                    }
                });
        });

        return services;
    }

    #endregion
}