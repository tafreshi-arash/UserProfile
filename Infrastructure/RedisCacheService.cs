using StackExchange.Redis;
using System.Text.Json;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _cache;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _cache = connectionMultiplexer.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _cache.StringSetAsync(key, json, expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _cache.StringGetAsync(key);
        return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
    }
}