using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Repositories;

public interface IPlayerDataPoolRepository
{
    Task<string?> PopAsync(GameType gameType, CancellationToken cancellationToken);
    Task PushAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken);
    Task RemoveAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken);
}