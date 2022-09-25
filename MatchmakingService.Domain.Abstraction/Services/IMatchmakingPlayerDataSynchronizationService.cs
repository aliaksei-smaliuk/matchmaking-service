using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface IMatchmakingPlayerDataSynchronizationService
{
    Task InitAsync(MatchmakingPlayerData target, CancellationToken cancellationToken);

    Task<bool> TryAddToOwnQueueAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner,
        CancellationToken cancellationToken);

    Task<bool> TryOwnAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner, TimeSpan ownTimeout,
        CancellationToken cancellationToken);

    Task ReleaseAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner, CancellationToken cancellationToken);
}