using MatchmakingService.DataAccess.Redis.Configurations;
using MatchmakingService.DataAccess.Redis.Interfaces;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using Microsoft.Extensions.Options;

namespace MatchmakingService.DataAccess.Redis.Repositories;

public class PlayerScoreRepository : IPlayerScoreRepository
{
    private readonly IOptions<RedisOptions> _redisOptions;
    private readonly IRedisService _redisService;

    public PlayerScoreRepository(
        IRedisService redisService,
        IOptions<RedisOptions> redisOptions)
    {
        _redisService = redisService;
        _redisOptions = redisOptions;
    }

    public async Task AddAsync(MatchmakingPlayerData matchmakingPlayerData, double score,
        CancellationToken cancellationToken)
    {
        await _redisService.SortedSetAddAsync(GetKey(matchmakingPlayerData.GameType), score,
            matchmakingPlayerData.RequestId, cancellationToken);
    }

    public async Task RemoveAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken)
    {
        await _redisService.SortedSetRemoveAsync(GetKey(matchmakingPlayerData.GameType),
            matchmakingPlayerData.RequestId, cancellationToken);
    }

    private string GetKey(GameType gameType) => $"{gameType}:{_redisOptions.Value.ScorePlayerPoolPath}";
}