using MatchmakingService.DataAccess.Redis.Configurations;
using MatchmakingService.DataAccess.Redis.Interfaces;
using MatchmakingService.Domain.Abstraction.Models;
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

    public async Task PushAsync(GameType gameType, string target, string owner, CancellationToken cancellationToken)
    {
        await _redisService.ListPushAsync(GetKey(gameType, target), owner, cancellationToken);
    }

    public async Task SetFirstAsync(GameType gameType, string target, string owner, CancellationToken cancellationToken)
    {
        await _redisService.ListSetFirstOrPushLeftAsync(GetKey(gameType, target), owner, cancellationToken);
    }

    public async Task<string?> GetAsync(GameType gameType, string target, int index,
        CancellationToken cancellationToken)
    {
        return await _redisService.ListGetAsync(GetKey(gameType, target), index, cancellationToken);
    }

    public async Task RemoveAsync(GameType gameType, string target, string owner, CancellationToken cancellationToken)
    {
        await _redisService.ListRemoveAsync(GetKey(gameType, target), owner, cancellationToken);
    }

    private string GetKey(GameType gameType, string key)
    {
        return $"{gameType}:{_redisOptions.Value.OwnerListPath}:{key}";
    }
}