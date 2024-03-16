namespace Shop.Application.Common.Interfaces;

public interface ICacheService
{
    Task<Result<T>> GetAsync<T>(string key);
    Task<Result> SetAsync<T>(string key, T value, TimeSpan expiry);
    Task<Result<T>> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiry);
}