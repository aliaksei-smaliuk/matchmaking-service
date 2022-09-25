using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface IOwnerMatchmakingService
{
    Task<OwnerMatchmakingResult> TryMatchmakingAsync(MatchmakingPlayerData matchmakingPlayerData,
        CancellationToken cancellationToken);
}