using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Repositories;

public interface IPlayerDataPoolRepository
{
    Task AddToActivityPoolAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken);
    Task AddToScorePoolAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken);
}