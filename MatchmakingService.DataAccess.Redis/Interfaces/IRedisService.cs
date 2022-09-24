namespace MatchmakingService.DataAccess.Redis.Interfaces;

public interface IRedisService
{
    Task AddAsync<T>(string key, T value, CancellationToken cancellationToken);
    Task AddToSortedSetAsync(string key, double score, string value, CancellationToken cancellationToken);
}