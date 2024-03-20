using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Infrastructure;

using Email;
using Identity;
using Data;
using Caching;
using Outbox;
using Authentication;
using Authorization;
using Application.Common.Interfaces;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<OutboxInterceptor>();

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            options.UseNpgsql(configuration["Database:ConnectionString"]);
            options.AddInterceptors(provider.GetRequiredService<OutboxInterceptor>());
            options.EnableSensitiveDataLogging();
        });

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ResourceAccess", policy =>
                policy.Requirements.Add(new ResourceAuthorizationRequirement()));
        });

        services.AddScoped<IResourceAuthorizationService, ResourceAuthorizationService>();
        services.AddSingleton<IAuthorizationHandler, AdminAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, OwnerAuthorizationHandler>();
        
        services.ConfigureOptions<JwtOptionsConfiguration>();
        services.ConfigureOptions<EmailOptionsConfiguration>();

        services.AddScoped<IUserService, UserService>();

        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddSingleton<IEmailService, EmailService>();

        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();

        services.AddHostedService<OutboxProcessor>();
        
        services.AddScoped<UserSeeder>();
        services.AddScoped<ApplicationDbSeeder>();

        return services;
    }
}