using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface IMatchmakingRequestService
{
    Task InitAsync(PlayerData playerData, CancellationToken cancellationToken);
    Task ClearAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken);
}