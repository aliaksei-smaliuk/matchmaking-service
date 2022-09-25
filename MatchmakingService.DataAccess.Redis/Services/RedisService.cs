using System.Text.Json;
using MatchmakingService.DataAccess.Redis.Configurations;
using MatchmakingService.DataAccess.Redis.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MatchmakingService.DataAccess.Redis.Services;

public class RedisService : IRedisService
{
    private readonly ThreadLocal<IDatabase> _databases;

    public RedisService(IOptions<RedisOptions> redisOptions)
    {
        var redis = ConnectionMultiplexer.Connect(redisOptions.Value.ConnectionString);
        _databases = new ThreadLocal<IDatabase>(() => redis.GetDatabase());
    }

    private IDatabase Database => _databases.Value!;

    public async Task AddAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        var valueStr = JsonSerializer.Serialize(value);
        await Database.StringSetAsync(key, valueStr).WaitAsync(cancellationToken);
    }

    public async Task SortedSetAddAsync(string key, double score, string value, CancellationToken cancellationToken)
    {
        var entry = new SortedSetEntry(value, score);
        await Database.SortedSetAddAsync(key, new[] {entry}).WaitAsync(cancellationToken);
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