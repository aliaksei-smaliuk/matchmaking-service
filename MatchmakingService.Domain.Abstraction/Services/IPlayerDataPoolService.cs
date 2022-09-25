using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface IPlayerDataPoolService
{
    Task<MatchmakingPlayerData?> PopAsync(GameType gameType, CancellationToken cancellationToken);
}