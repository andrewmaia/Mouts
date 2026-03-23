namespace Mouts.Application.Interfaces;

public interface ICache
{
    Task<T?> GetAsync<T>(string key);

    Task SetAsync<T>(string key, T value, TimeSpan ttl);

    Task<T> GetOrCreateAsync<T>(string key,TimeSpan ttl,Func<Task<T>> factory);
}
