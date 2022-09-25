using MatchmakingService.DataAccess.Redis.Configurations;
using MatchmakingService.DataAccess.Redis.Interfaces;
using MatchmakingService.Domain.Abstraction.Repositories;
using Microsoft.Extensions.Options;

namespace MatchmakingService.DataAccess.Redis.Repositories;

public class OwnerListRepository : IOwnerListRepository
{
    private readonly IOptions<RedisOptions> _redisOptions;
    private readonly IRedisService _redisService;

    public OwnerListRepository(IRedisService redisService, IOptions<RedisOptions> redisOptions)
    {
        _redisService = redisService;
        _redisOptions = redisOptions;
    }

    public async Task PushAsync(string target, string owner, CancellationToken cancellationToken)
    {
        await _redisService.ListPushAsync(GetKey(target), owner, cancellationToken);
    }

    public async Task SetFirstAsync(string target, string owner, CancellationToken cancellationToken)
    {
        await _redisService.ListSetFirstOrPushLeftAsync(GetKey(target), owner, cancellationToken);
    }

    public async Task<string?> GetAsync(string target, int index, CancellationToken cancellationToken)
    {
        return await _redisService.ListGetAsync(GetKey(target), index, cancellationToken);
    }

    public async Task RemoveAsync(string target, string owner, CancellationToken cancellationToken)
    {
        await _redisService.ListRemoveAsync(GetKey(target), owner, cancellationToken);
    }

    private string GetKey(string key)
    {
        return $"{_redisOptions.Value.OwnerListPath}:{key}";
    }
}