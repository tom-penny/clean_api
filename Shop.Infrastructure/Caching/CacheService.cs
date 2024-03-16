using Microsoft.Extensions.Caching.Memory;

namespace Shop.Infrastructure.Caching;

using Application.Common.Interfaces;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task<Result<T>> GetAsync<T>(string key)
    {
        if (_memoryCache.TryGetValue(key, out T? value) && value != null)
        {
            return Task.FromResult(Result.Ok<T>(value));
        }
        
        return Task.FromResult(Result.Fail<T>("Cache miss"));
    }

    public Task<Result> SetAsync<T>(string key, T value, TimeSpan expiry)
    {
        _memoryCache.Set(key, value, expiry);

        return Task.FromResult(Result.Ok());
    }

    public async Task<Result<T>> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiry)
    {
        var value = await _memoryCache.GetOrCreateAsync(key, entry =>
        {
            entry.SetAbsoluteExpiration(expiry);
            
            return factory();
        });
        
        return value != null ? Result.Ok(value) : Result.Fail("");
    }
}