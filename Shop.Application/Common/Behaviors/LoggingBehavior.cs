using Microsoft.Extensions.Logging;

namespace Shop.Application.Behaviors;

// Pipeline interceptor for Mediatr request logging.

public class LoggingBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResultBase
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{@DateTime}: Started {@Request}",
            typeof(TRequest).Name, DateTime.UtcNow);
        
        var response = await next();

        if (response.IsSuccess)
        {
            _logger.LogInformation("{@DateTime}: Completed {@Request}",
                typeof(TRequest).Name, DateTime.UtcNow);    
        }
        else
        {
            _logger.LogWarning("{@DateTime}: Failed {@Request}",
                typeof(TRequest).Name, DateTime.UtcNow);
        }

        return response;
    }
}