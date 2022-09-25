using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Services;

namespace MatchmakingService.Domain.Services;

public class OwnerMatchmakingService : IOwnerMatchmakingService
{
    private readonly IMatchmakingPlayerDataSynchronizationService _synchronizationService;

    public OwnerMatchmakingService(IMatchmakingPlayerDataSynchronizationService synchronizationService)
    {
        _synchronizationService = synchronizationService;
    }

    public async Task<OwnerMatchmakingResult> TryMatchmakingAsync(MatchmakingPlayerData matchmakingPlayerData,
        CancellationToken cancellationToken)
    {
        var addToSelfOwnQueueResult = await TryAddToSelfOwnQueueAsync(matchmakingPlayerData, cancellationToken);
        if (!addToSelfOwnQueueResult)
            return OwnerMatchmakingResult.Fail;

        // Database Filtering
        var largeCandidatesSample = await GetLargeCandidatesSampleAsync(matchmakingPlayerData, cancellationToken);

        // Add player to own queue for all candidates
        var ownedLargeCandidatesSample =
            await TryAddToOwnQueueAsync(largeCandidatesSample, matchmakingPlayerData, cancellationToken);

        // Filter in-memory using some algorithm (could be long-running (e.g. Genetic algorithm, Simulated annealing))
        var smallCandidatesSample =
            await FilterCandidatesAsync(ownedLargeCandidatesSample, matchmakingPlayerData, cancellationToken);

        // Own candidates to complete room
        var (roomCompleted, ownedCandidates) =
            await OwnCandidatesAsync(smallCandidatesSample, matchmakingPlayerData, cancellationToken);
        if (!roomCompleted)
        {
            // Release all candidates
            await ReleaseCandidatesAsync(ownedLargeCandidatesSample, matchmakingPlayerData, cancellationToken);
            return OwnerMatchmakingResult.Fail;
        }

        // Release candidates that are not in the room
        await ReleaseCandidatesAsync(ownedLargeCandidatesSample.Except(ownedCandidates).ToArray(),
            matchmakingPlayerData,
            cancellationToken);
        return OwnerMatchmakingResult.Success(ownedCandidates.Select(c => c.PlayerId).ToArray());
    }

    private async Task<bool> TryAddToSelfOwnQueueAsync(MatchmakingPlayerData matchmakingPlayerData,
        CancellationToken cancellationToken)
    {
        return await _synchronizationService.TryAddToOwnQueueAsync(matchmakingPlayerData, matchmakingPlayerData,
            cancellationToken);
    }

    private async Task<IReadOnlyCollection<MatchmakingPlayerData>> GetLargeCandidatesSampleAsync(
        MatchmakingPlayerData owner, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<IReadOnlyCollection<MatchmakingPlayerData>> TryAddToOwnQueueAsync(
        IReadOnlyCollection<MatchmakingPlayerData> candidates, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<IReadOnlyCollection<MatchmakingPlayerData>> FilterCandidatesAsync(
        IReadOnlyCollection<MatchmakingPlayerData> candidates, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<(bool, IReadOnlyCollection<MatchmakingPlayerData>)> OwnCandidatesAsync(
        IReadOnlyCollection<MatchmakingPlayerData> smallCandidatesSample, MatchmakingPlayerData matchmakingPlayerData,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task ReleaseCandidatesAsync(IReadOnlyCollection<MatchmakingPlayerData> ownedLargeCandidatesSample,
        MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}