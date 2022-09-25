using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface ICandidatesFilterService
{
    Task<IReadOnlyCollection<MatchmakingPlayerData>> FilterCandidatesAsync(
        IReadOnlyCollection<MatchmakingPlayerData> candidates, MatchmakingPlayerData owner,
        CancellationToken cancellationToken);
}