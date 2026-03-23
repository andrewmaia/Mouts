using Mouts.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;


namespace Mouts.Infrastructure.Services.Caching;


public sealed class MemoryCacheService : ICache
{
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        _cache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan ttl)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        };

        _cache.Set(key, value, options);
        return Task.CompletedTask;
    }

    public async Task<T> GetOrCreateAsync<T>(string key, TimeSpan ttl,Func<Task<T>> factory)
    {
        if (_cache.TryGetValue(key, out T? cached) && cached is not null)
            return cached;

        var value = await factory();

        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        };

        _cache.Set(key, value, options);

        return value;
    }
}
