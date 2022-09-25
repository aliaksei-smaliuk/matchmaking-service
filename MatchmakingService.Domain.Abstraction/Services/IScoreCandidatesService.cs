using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface IScoreCandidatesService
{
    Task<IReadOnlyCollection<MatchmakingPlayerData>> GetLargeCandidatesSampleAsync(MatchmakingPlayerData owner,
        CancellationToken cancellationToken);
}