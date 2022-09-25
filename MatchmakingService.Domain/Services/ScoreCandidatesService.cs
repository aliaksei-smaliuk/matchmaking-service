using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Services;

namespace MatchmakingService.Domain.Services;

public class ScoreCandidatesService : IScoreCandidatesService
{
    public Task<IReadOnlyCollection<MatchmakingPlayerData>> GetLargeCandidatesSampleAsync(MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}