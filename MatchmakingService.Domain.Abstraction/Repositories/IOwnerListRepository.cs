namespace MatchmakingService.Domain.Abstraction.Repositories;

public interface IOwnerListRepository
{
    Task PushAsync(string target, string owner, CancellationToken cancellationToken);
    Task SetFirstAsync(string target, string owner, CancellationToken cancellationToken);
    Task<string?> GetAsync(string target, int index, CancellationToken cancellationToken);
    Task RemoveAsync(string target, string owner, CancellationToken cancellationToken);
}