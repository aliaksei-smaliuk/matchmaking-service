using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface IMatchmakingPlayerDataOwnerListService
{
    Task<bool> IsActiveAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner,
        CancellationToken cancellationToken);

    Task PushAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner, CancellationToken cancellationToken);
    Task RemoveAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner, CancellationToken cancellationToken);

    Task ActivateAsync(MatchmakingPlayerData target, CancellationToken cancellationToken);
    Task DeactivateAsync(MatchmakingPlayerData target, CancellationToken cancellationToken);
    Task<bool> IsActiveAsync(MatchmakingPlayerData target, CancellationToken cancellationToken);
}