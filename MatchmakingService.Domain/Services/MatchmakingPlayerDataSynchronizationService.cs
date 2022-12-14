using MatchmakingService.Domain.Abstraction.Configuration;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Services;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace MatchmakingService.Domain.Services;

public class MatchmakingPlayerDataSynchronizationService : IMatchmakingPlayerDataSynchronizationService
{
    private readonly IMatchmakingPlayerDataOwnerListService _ownerListService;
    private readonly AsyncRetryPolicy<bool> _ownPolicy;

    public MatchmakingPlayerDataSynchronizationService(IMatchmakingPlayerDataOwnerListService ownerListService,
        IOptions<MatchmakingOptions> matchmakingOptions)
    {
        _ownerListService = ownerListService;

        var ownOptions = matchmakingOptions.Value.OwnOptions;
        _ownPolicy = Policy
            .HandleResult(false)
            .WaitAndRetryAsync(ownOptions.OwnRetriesCount, _ => ownOptions.OwnTimeout / ownOptions.OwnRetriesCount);
    }

    public async Task InitAsync(MatchmakingPlayerData target, CancellationToken cancellationToken)
    {
        await _ownerListService.ActivateAsync(target, cancellationToken);
    }

    public async Task<bool> TryAddToOwnQueueAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        var isActive = await _ownerListService.IsActiveAsync(target, cancellationToken);
        if (!isActive)
            return false;

        await _ownerListService.PushAsync(target, owner, cancellationToken);
        return true;
    }

    public async Task<bool> CanOwnAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        var isActive = await _ownerListService.IsActiveAsync(target, cancellationToken);
        if (!isActive)
            return false;

        var isOwn = await _ownPolicy.ExecuteAsync(() =>
            _ownerListService.IsActiveAsync(target, owner, cancellationToken));

        return isOwn;
    }

    public async Task OwnAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        var isOwn = await CanOwnAsync(target, owner, cancellationToken);

        if (isOwn)
            await _ownerListService.DeactivateAsync(target, cancellationToken);
        else
            throw new InvalidOperationException("Cannot own with other owner");
    }

    public async Task ReleaseAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        await _ownerListService.RemoveAsync(target, owner, cancellationToken);
    }
}