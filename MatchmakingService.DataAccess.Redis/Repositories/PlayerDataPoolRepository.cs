using MatchmakingService.DataAccess.Redis.Configurations;
using MatchmakingService.DataAccess.Redis.Interfaces;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace MatchmakingService.DataAccess.Redis.Repositories;

public class PlayerDataPoolRepository : IPlayerDataPoolRepository
{
    private readonly IOptions<RedisOptions> _redisOptions;
    private readonly IRedisService _redisService;
    private readonly ISystemClock _systemClock;

    public PlayerDataPoolRepository(
        IRedisService redisService,
        IOptions<RedisOptions> redisOptions,
        ISystemClock systemClock)
    {
        _redisService = redisService;
        _redisOptions = redisOptions;
        _systemClock = systemClock;
    }

    public async Task<string?> PopAsync(GameType gameType, CancellationToken cancellationToken)
    {
        return await _redisService.SortedSetPopAsync(GetKey(gameType), cancellationToken);
    }

    public async Task PushAsync(MatchmakingPlayerData matchmakingPlayerData,
        CancellationToken cancellationToken)
    {
        var score = _systemClock.UtcNow.ToUnixTimeMilliseconds();
        await _redisService.SortedSetAddAsync(GetKey(matchmakingPlayerData.GameType), score,
            matchmakingPlayerData.RequestId, cancellationToken);
    }

    public async Task RemoveAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken)
    {
        await _redisService.SortedSetRemoveAsync(GetKey(matchmakingPlayerData.GameType),
            matchmakingPlayerData.RequestId, cancellationToken);
    }

    private string GetKey(GameType gameType) => $"{gameType}:{_redisOptions.Value.ActivityPlayerPoolPath}";
}