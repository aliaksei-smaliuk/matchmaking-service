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

    public async Task AddToSortedSetAsync(string key, double score, string value, CancellationToken cancellationToken)
    {
        var entry = new SortedSetEntry(value, score);
        await Database.SortedSetAddAsync(key, new[] {entry}).WaitAsync(cancellationToken);
    }
}