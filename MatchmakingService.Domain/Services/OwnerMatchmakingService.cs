using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Services;

namespace MatchmakingService.Domain.Services;

public class OwnerMatchmakingService : IOwnerMatchmakingService
{
    private readonly ICandidatesFilterService _candidatesFilterService;
    private readonly IOwnCandidatesService _ownCandidatesService;
    private readonly IScoreCandidatesService _scoreCandidatesService;
    private readonly IMatchmakingPlayerDataSynchronizationService _synchronizationService;

    public OwnerMatchmakingService(IScoreCandidatesService scoreCandidatesService,
        IMatchmakingPlayerDataSynchronizationService synchronizationService,
        ICandidatesFilterService candidatesFilterService,
        IOwnCandidatesService ownCandidatesService)
    {
        _scoreCandidatesService = scoreCandidatesService;
        _synchronizationService = synchronizationService;
        _candidatesFilterService = candidatesFilterService;
        _ownCandidatesService = ownCandidatesService;
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
        var ownCandidatesResult =
            await OwnCandidatesAsync(smallCandidatesSample, matchmakingPlayerData, cancellationToken);
        if (!ownCandidatesResult.IsSuccess)
        {
            // Release all candidates
            await ReleaseCandidatesAsync(ownedLargeCandidatesSample, matchmakingPlayerData, cancellationToken);
            return OwnerMatchmakingResult.Fail;
        }

        // Release candidates that are not in the room
        var ownedCandidates = ownCandidatesResult.OwnedCandidates!;
        await ReleaseCandidatesAsync(ownedLargeCandidatesSample.Except(ownedCandidates).ToArray(),
            matchmakingPlayerData, cancellationToken);
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
        return await _scoreCandidatesService.GetLargeCandidatesSampleAsync(owner, cancellationToken);
    }

    private async Task<IReadOnlyCollection<MatchmakingPlayerData>> TryAddToOwnQueueAsync(
        IReadOnlyCollection<MatchmakingPlayerData> candidates, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        var candidate2QueueOwnTask = candidates.Where(c => c.RequestId != owner.RequestId).ToDictionary(c => c,
            c => _synchronizationService.TryAddToOwnQueueAsync(c, owner, cancellationToken));
        await Task.WhenAll(candidate2QueueOwnTask.Values);

        return candidate2QueueOwnTask
            .Where(p => p.Value.Result)
            .Select(p => p.Key)
            .Concat(new[] {owner})
            .ToArray();
    }

    private async Task<IReadOnlyCollection<MatchmakingPlayerData>> FilterCandidatesAsync(
        IReadOnlyCollection<MatchmakingPlayerData> candidates, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        return await _candidatesFilterService.FilterCandidatesAsync(candidates, owner, cancellationToken);
    }

    private async Task<OwnCandidatesResult> OwnCandidatesAsync(
        IReadOnlyCollection<MatchmakingPlayerData> candidatesSample, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        return await _ownCandidatesService.OwnCandidatesAsync(candidatesSample, owner, cancellationToken);
    }

    private async Task ReleaseCandidatesAsync(IReadOnlyCollection<MatchmakingPlayerData> ownedCandidatesSample,
        MatchmakingPlayerData owner, CancellationToken cancellationToken)
    {
        var releaseTasks = ownedCandidatesSample.Select(c =>
            _synchronizationService.ReleaseAsync(c, owner, cancellationToken));
        await Task.WhenAll(releaseTasks);
    }
}