using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface IOwnCandidatesService
{
    Task<OwnCandidatesResult> OwnCandidatesAsync(
        IReadOnlyCollection<MatchmakingPlayerData> candidatesSample, MatchmakingPlayerData owner,
        CancellationToken cancellationToken);
}