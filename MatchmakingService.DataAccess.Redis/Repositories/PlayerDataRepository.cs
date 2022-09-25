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

    public async Task<MatchmakingPlayerData?> GetAsync(GameType gameType, string requestId,
        CancellationToken cancellationToken)
    {
        return await _redisService.GetAsync<MatchmakingPlayerData>(GetKey(gameType, requestId), cancellationToken);
    }

    public async Task<IReadOnlyCollection<MatchmakingPlayerData>> GetAsync(GameType gameType,
        IReadOnlyCollection<string> requestIds, CancellationToken cancellationToken)
    {
        var keys = requestIds.Select(i => GetKey(gameType, i)).ToArray();
        return await _redisService.GetAsync<MatchmakingPlayerData>(keys, cancellationToken);
    }

    private string GetKey(MatchmakingPlayerData matchmakingPlayerData) =>
        GetKey(matchmakingPlayerData.GameType, matchmakingPlayerData.RequestId);

    private string GetKey(GameType gameType, string key)
    {
        return $"{gameType}:{_redisOptions.Value.MatchMakingPlayerDataPath}:{key}";
    }
}