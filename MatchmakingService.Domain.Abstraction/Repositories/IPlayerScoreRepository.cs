using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Repositories;

public interface IPlayerScoreRepository
{
    Task AddAsync(MatchmakingPlayerData matchmakingPlayerData, double score, CancellationToken cancellationToken);
    Task RemoveAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken);
}