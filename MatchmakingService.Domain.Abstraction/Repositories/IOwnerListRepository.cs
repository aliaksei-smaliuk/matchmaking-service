using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Repositories;

public interface IOwnerListRepository
{
    Task PushAsync(GameType gameType, string target, string owner, CancellationToken cancellationToken);
    Task SetFirstAsync(GameType gameType, string target, string owner, CancellationToken cancellationToken);
    Task<string?> GetAsync(GameType gameType, string target, int index, CancellationToken cancellationToken);
    Task RemoveAsync(GameType gameType, string target, string owner, CancellationToken cancellationToken);
}