using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Services;

namespace MatchmakingService.Domain.Services;

public class CandidatesFilterService : ICandidatesFilterService
{
    public Task<IReadOnlyCollection<MatchmakingPlayerData>> FilterCandidatesAsync(
        IReadOnlyCollection<MatchmakingPlayerData> candidates, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        // TODO Perform some complicated calculation
        var filteredCandidates = candidates
            .Where(c => c.Platform == owner.Platform)
            .ToArray();
        return Task.FromResult<IReadOnlyCollection<MatchmakingPlayerData>>(filteredCandidates);
    }
}