using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Repositories;

public interface IPlayerDataRepository
{
    Task AddAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken);
}