using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using MatchmakingService.Domain.Abstraction.Services;

namespace MatchmakingService.Domain.Services;

public class MatchmakingPlayerDataOwnerListService : IMatchmakingPlayerDataOwnerListService
{
    private const int StatusDataIndex = 0;
    private const int CurrentOwnerIndex = 1;
    private const string ActiveStatusName = "$ACTIVE$";
    private const string InactiveStatusName = "$INACTIVE$";

    private readonly IOwnerListRepository _ownerListRepository;

    public MatchmakingPlayerDataOwnerListService(IOwnerListRepository ownerListRepository)
    {
        _ownerListRepository = ownerListRepository;
    }

    public async Task<bool> IsActiveAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        var ownerId = await _ownerListRepository.GetAsync(target.RequestId, CurrentOwnerIndex, cancellationToken);
        return owner.RequestId.Equals(ownerId, StringComparison.Ordinal);
    }

    public async Task PushAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        await _ownerListRepository.PushAsync(target.RequestId, owner.RequestId, cancellationToken);
    }

    public async Task RemoveAsync(MatchmakingPlayerData target, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        await _ownerListRepository.RemoveAsync(target.RequestId, owner.RequestId, cancellationToken);
    }

    public async Task ActivateAsync(MatchmakingPlayerData target, CancellationToken cancellationToken)
    {
        await _ownerListRepository.SetFirstAsync(target.RequestId, ActiveStatusName, cancellationToken);
    }

    public async Task DeactivateAsync(MatchmakingPlayerData target, CancellationToken cancellationToken)
    {
        await _ownerListRepository.SetFirstAsync(target.RequestId, InactiveStatusName, cancellationToken);
    }

    public async Task<bool> IsActiveAsync(MatchmakingPlayerData target, CancellationToken cancellationToken)
    {
        var status = await _ownerListRepository.GetAsync(target.RequestId, StatusDataIndex, cancellationToken);
        return ActiveStatusName.Equals(status, StringComparison.Ordinal);
    }
}