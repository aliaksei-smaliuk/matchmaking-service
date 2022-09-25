using MatchmakingService.Domain.Abstraction.Configuration;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using MatchmakingService.Domain.Abstraction.Services;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace MatchmakingService.Domain.Services;

public class MatchmakingInitializationService : IMatchmakingInitializationService
{
    private readonly IOptions<MatchmakingOptions> _matchmakingOptions;
    private readonly IPlayerDataPoolRepository _playerDataPoolRepository;
    private readonly IPlayerDataRepository _playerDataRepository;
    private readonly IMatchmakingPlayerDataSynchronizationService _synchronizationService;
    private readonly ISystemClock _systemClock;

    public MatchmakingInitializationService(
        IPlayerDataPoolRepository playerDataPoolRepository,
        IPlayerDataRepository playerDataRepository,
        IMatchmakingPlayerDataSynchronizationService synchronizationService,
        ISystemClock systemClock,
        IOptions<MatchmakingOptions> matchmakingOptions)
    {
        _playerDataPoolRepository = playerDataPoolRepository;
        _playerDataRepository = playerDataRepository;
        _synchronizationService = synchronizationService;
        _systemClock = systemClock;
        _matchmakingOptions = matchmakingOptions;
    }

    public async Task AddRequestAsync(PlayerData playerData, CancellationToken cancellationToken)
    {
        var matchmakingPlayerData = new MatchmakingPlayerData(playerData)
            {ValidUntil = _systemClock.UtcNow + _matchmakingOptions.Value.MatchmakingTimeout};

        // Important before all other initializations
        await _synchronizationService.InitAsync(matchmakingPlayerData, cancellationToken);

        await Task.WhenAll(
            _playerDataPoolRepository.AddToActivityPoolAsync(matchmakingPlayerData, cancellationToken),
            _playerDataPoolRepository.AddToScorePoolAsync(matchmakingPlayerData, cancellationToken),
            _playerDataRepository.AddAsync(matchmakingPlayerData, cancellationToken)
        );
    }
}