using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shop.API;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireUser", policy =>
                policy.RequireRole("User"));
            options.AddPolicy("RequireAdmin", policy =>
                policy.RequireRole("Admin"));
            options.AddPolicy("RequireLogin", policy =>
                policy.RequireAuthenticatedUser());
        });        
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen();
        
        return services;
    }
}