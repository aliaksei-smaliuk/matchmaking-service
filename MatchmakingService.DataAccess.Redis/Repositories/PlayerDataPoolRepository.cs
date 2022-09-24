using MatchmakingService.DataAccess.Redis.Configurations;
using MatchmakingService.DataAccess.Redis.Interfaces;
using MatchmakingService.Domain.Abstraction.Configuration;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace MatchmakingService.DataAccess.Redis.Repositories;

public class PlayerDataPoolRepository : IPlayerDataPoolRepository
{
    private readonly IOptions<GameOptions> _gameOptions;
    private readonly IOptions<RedisOptions> _redisOptions;
    private readonly IRedisService _redisService;
    private readonly ISystemClock _systemClock;

    public PlayerDataPoolRepository(
        IRedisService redisService,
        IOptions<RedisOptions> redisOptions,
        IOptions<GameOptions> gameOptions,
        ISystemClock systemClock)
    {
        _redisService = redisService;
        _redisOptions = redisOptions;
        _gameOptions = gameOptions;
        _systemClock = systemClock;
    }

    public async Task AddToActivityPoolAsync(MatchmakingPlayerData matchmakingPlayerData,
        CancellationToken cancellationToken)
    {
        var score = -_systemClock.UtcNow.ToUnixTimeMilliseconds();
        await _redisService.AddToSortedSetAsync(_redisOptions.Value.ActivityPlayerPoolPath, score,
            matchmakingPlayerData.PlayerId, cancellationToken);
    }

    public async Task AddToScorePoolAsync(MatchmakingPlayerData matchmakingPlayerData,
        CancellationToken cancellationToken)
    {
        var score = matchmakingPlayerData.Level +
                    (double) matchmakingPlayerData.Cash / _gameOptions.Value.MaxCashAmount;
        await _redisService.AddToSortedSetAsync(_redisOptions.Value.ScorePlayerPoolPath, score,
            matchmakingPlayerData.PlayerId, cancellationToken);
    }
}