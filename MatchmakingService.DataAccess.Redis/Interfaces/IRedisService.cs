namespace MatchmakingService.DataAccess.Redis.Interfaces;

public interface IRedisService
{
    Task AddAsync<T>(string key, T value, CancellationToken cancellationToken);
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class;
    Task SortedSetAddAsync(string key, double score, string value, CancellationToken cancellationToken);
    Task<string?> SortedSetPopAsync(string key, CancellationToken cancellationToken);
    Task SortedSetRemoveAsync(string key, string value, CancellationToken cancellationToken);
    Task ListPushAsync(string key, string value, CancellationToken cancellationToken);
    Task ListSetFirstOrPushLeftAsync(string key, string value, CancellationToken cancellationToken);
    Task<string?> ListGetAsync(string key, int index, CancellationToken cancellationToken);
    Task ListRemoveAsync(string key, string value, CancellationToken cancellationToken);
}