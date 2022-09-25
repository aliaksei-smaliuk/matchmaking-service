using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Repositories;

public interface IPlayerDataRepository
{
    Task AddAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken);
    Task<MatchmakingPlayerData?> GetAsync(GameType gameType, string requestId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<MatchmakingPlayerData>> GetAsync(GameType gameType, IReadOnlyCollection<string> requestIds,
        CancellationToken cancellationToken);
}