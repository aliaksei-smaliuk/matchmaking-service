using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface IMatchmakingPlayerDataSynchronizationService
{
    Task InitAsync(MatchmakingPlayerData target, CancellationToken cancellationToken);

    Task<bool> TryAddToOwnQueueAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner,
        CancellationToken cancellationToken);

    Task<bool> CanOwnAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner,
        CancellationToken cancellationToken);

    Task OwnAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner, CancellationToken cancellationToken);

    Task ReleaseAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner, CancellationToken cancellationToken);
}