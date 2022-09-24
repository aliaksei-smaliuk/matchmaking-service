using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface IPlayerMatchmakingService
{
    Task AddPlayerToQueueAsync(PlayerData playerData, CancellationToken cancellationToken);
}