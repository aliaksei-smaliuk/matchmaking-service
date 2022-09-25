using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface IGeneralMatchmakingService
{
    Task<GeneralMatchmakingResult> TryMatchmakingAsync(GameType gameType, CancellationToken cancellationToken);
}