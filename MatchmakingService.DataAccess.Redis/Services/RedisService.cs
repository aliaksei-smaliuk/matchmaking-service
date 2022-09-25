using System.Text.Json;
using MatchmakingService.DataAccess.Redis.Configurations;
using MatchmakingService.DataAccess.Redis.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MatchmakingService.DataAccess.Redis.Services;

public class RedisService : IRedisService
{
    private readonly ThreadLocal<IDatabase> _databases;
    private readonly ILogger<RedisService> _logger;

    public RedisService(IOptions<RedisOptions> redisOptions, ILogger<RedisService> logger)
    {
        _logger = logger;

        var redis = ConnectionMultiplexer.Connect(redisOptions.Value.ConnectionString);
        _databases = new ThreadLocal<IDatabase>(() => redis.GetDatabase());
    }

    private IDatabase Database => _databases.Value!;

    public async Task AddAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        var valueStr = JsonSerializer.Serialize(value);
        await Database.StringSetAsync(key, valueStr).WaitAsync(cancellationToken);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class
    {
        var redisValue = await Database.StringGetAsync(key).WaitAsync(cancellationToken);
        var valueStr = (string?) redisValue;
        if (valueStr is null)
            return null;
        try
        {
            return JsonSerializer.Deserialize<T>(valueStr);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get data by key {Key}", key);
            return null;
        }
    }

    public async Task SortedSetAddAsync(string key, double score, string value, CancellationToken cancellationToken)
    {
        var entry = new SortedSetEntry(value, score);
        await Database.SortedSetAddAsync(key, new[] {entry}).WaitAsync(cancellationToken);
    }

    public async Task<string?> SortedSetPopAsync(string key, CancellationToken cancellationToken)
    {
        var value = await Database.SortedSetPopAsync(key).WaitAsync(cancellationToken);
        return value is null ? null : (string?) value.Value.Element;
    }

    public async Task SortedSetRemoveAsync(string key, string value, CancellationToken cancellationToken)
    {
        await Database.SortedSetRemoveAsync(key, value).WaitAsync(cancellationToken);
    }

    public async Task ListPushAsync(string key, string value, CancellationToken cancellationToken)
    {
        await Database.ListRightPushAsync(key, value).WaitAsync(cancellationToken);
    }

    public async Task ListSetFirstOrPushLeftAsync(string key, string value, CancellationToken cancellationToken)
    {
        var length = await Database.ListLengthAsync(key).WaitAsync(cancellationToken);
        if (length > 0)
            await Database.ListSetByIndexAsync(key, 0, value).WaitAsync(cancellationToken);
        else
            await Database.ListLeftPushAsync(key, value).WaitAsync(cancellationToken);
    }

    public async Task<string?> ListGetAsync(string key, int index, CancellationToken cancellationToken)
    {
        return await Database.ListGetByIndexAsync(key, index).WaitAsync(cancellationToken);
    }

    public async Task ListRemoveAsync(string key, string value, CancellationToken cancellationToken)
    {
        await Database.ListRemoveAsync(key, value).WaitAsync(cancellationToken);
    }
}