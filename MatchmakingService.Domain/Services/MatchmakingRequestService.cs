using MatchmakingService.Domain.Abstraction.Configuration;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using MatchmakingService.Domain.Abstraction.Services;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace MatchmakingService.Domain.Services;

public class MatchmakingRequestService : IMatchmakingRequestService
{
    private readonly IOptions<MatchmakingOptions> _matchmakingOptions;
    private readonly IMatchmakingScoreService _matchmakingScoreService;
    private readonly IMatchmakingPlayerDataOwnerListService _ownerListService;
    private readonly IPlayerDataPoolRepository _playerDataPoolRepository;
    private readonly IPlayerDataRepository _playerDataRepository;
    private readonly IPlayerScoreRepository _playerScoreRepository;
    private readonly IMatchmakingPlayerDataSynchronizationService _synchronizationService;
    private readonly ISystemClock _systemClock;

    public MatchmakingRequestService(
        IPlayerDataPoolRepository playerDataPoolRepository,
        IPlayerScoreRepository playerScoreRepository,
        IPlayerDataRepository playerDataRepository,
        IMatchmakingPlayerDataSynchronizationService synchronizationService,
        IMatchmakingScoreService matchmakingScoreService,
        IMatchmakingPlayerDataOwnerListService ownerListService,
        ISystemClock systemClock,
        IOptions<MatchmakingOptions> matchmakingOptions)
    {
        _playerDataPoolRepository = playerDataPoolRepository;
        _playerScoreRepository = playerScoreRepository;
        _playerDataRepository = playerDataRepository;
        _synchronizationService = synchronizationService;
        _matchmakingScoreService = matchmakingScoreService;
        _ownerListService = ownerListService;
        _systemClock = systemClock;
        _matchmakingOptions = matchmakingOptions;
    }

    public async Task InitAsync(PlayerData playerData, CancellationToken cancellationToken)
    {
        var matchmakingPlayerData = new MatchmakingPlayerData(playerData)
            {ValidUntil = _systemClock.UtcNow + _matchmakingOptions.Value.MatchmakingTimeout};

        // Important before all other initializations
        await _synchronizationService.InitAsync(matchmakingPlayerData, cancellationToken);

        await Task.WhenAll(
            _playerDataPoolRepository.PushAsync(matchmakingPlayerData, cancellationToken),
            _playerScoreRepository.AddAsync(matchmakingPlayerData,
                _matchmakingScoreService.GetScore(matchmakingPlayerData), cancellationToken),
            _playerDataRepository.AddAsync(matchmakingPlayerData, cancellationToken)
        );
    }

    public async Task ClearAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken)
    {
        await _ownerListService.DeactivateAsync(matchmakingPlayerData, cancellationToken);

        await Task.WhenAll(
            _playerScoreRepository.RemoveAsync(matchmakingPlayerData, cancellationToken),
            _playerDataPoolRepository.RemoveAsync(matchmakingPlayerData, cancellationToken)
        );
    }
}