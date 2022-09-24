using MatchmakingService.Domain.Abstraction.Configuration;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using MatchmakingService.Domain.Abstraction.Services;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace MatchmakingService.Domain.Services;

public class PlayerMatchmakingService : IPlayerMatchmakingService
{
    private readonly IOptions<GameOptions> _gameOptions;
    private readonly IPlayerDataPoolRepository _playerDataPoolRepository;
    private readonly IPlayerDataRepository _playerDataRepository;
    private readonly ISystemClock _systemClock;

    public PlayerMatchmakingService(
        IPlayerDataPoolRepository playerDataPoolRepository,
        IPlayerDataRepository playerDataRepository,
        ISystemClock systemClock,
        IOptions<GameOptions> gameOptions)
    {
        _playerDataPoolRepository = playerDataPoolRepository;
        _playerDataRepository = playerDataRepository;
        _systemClock = systemClock;
        _gameOptions = gameOptions;
    }

    public async Task AddPlayerToQueueAsync(PlayerData playerData, CancellationToken cancellationToken)
    {
        var matchmakingPlayerData = new MatchmakingPlayerData(playerData)
            {ValidUntil = _systemClock.UtcNow + _gameOptions.Value.MatchmakingTimeout};

        await Task.WhenAll(
            _playerDataPoolRepository.AddToActivityPoolAsync(matchmakingPlayerData, cancellationToken),
            _playerDataPoolRepository.AddToScorePoolAsync(matchmakingPlayerData, cancellationToken),
            _playerDataRepository.AddAsync(matchmakingPlayerData, cancellationToken)
        );
    }
}