using MatchmakingService.DataAccess.Redis.Configurations;
using MatchmakingService.DataAccess.Redis.Interfaces;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using Microsoft.Extensions.Options;

namespace MatchmakingService.DataAccess.Redis.Repositories;

public class PlayerDataRepository : IPlayerDataRepository
{
    private readonly IOptions<RedisOptions> _redisOptions;
    private readonly IRedisService _redisService;

    public PlayerDataRepository(
        IRedisService redisService,
        IOptions<RedisOptions> redisOptions)
    {
        _redisService = redisService;
        _redisOptions = redisOptions;
    }

    public async Task AddAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken)
    {
        await _redisService.AddAsync(GetKey(matchmakingPlayerData), matchmakingPlayerData, cancellationToken);
    }

    private string GetKey(MatchmakingPlayerData matchmakingPlayerData)
    {
        return $"{_redisOptions.Value.MatchMakingPlayerDataPath}:{matchmakingPlayerData.PlayerId}";
    }
}