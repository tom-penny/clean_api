using Microsoft.Extensions.DependencyInjection;

namespace Shop.Application;

using Common.Behaviors;

// Service registration for Order.Application dependencies.

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register Mediatr in-process messaging.

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        });
        
        services.AddValidatorsFromAssembly(typeof(ConfigureServices).Assembly, includeInternalTypes: true);
        
        return services;
    }
}