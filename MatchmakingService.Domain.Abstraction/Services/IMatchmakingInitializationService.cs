using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface IMatchmakingInitializationService
{
    Task AddRequestAsync(PlayerData playerData, CancellationToken cancellationToken);
}