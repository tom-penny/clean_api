namespace Shop.Application.Common.Behaviors;

using Interfaces;

// Pipeline interceptor for Mediatr request caching.

public class CachingBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery<TResponse>
    where TResponse : IResultBase
{
    private readonly ICacheService _cacheService;

    public CachingBehavior(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await _cacheService.GetAsync<TResponse>(request.Key);

        if (result.IsSuccess) return result.Value;

        var response = await next();

        await _cacheService.SetAsync(request.Key, response, request.Expiry);

        return response;
    }
}