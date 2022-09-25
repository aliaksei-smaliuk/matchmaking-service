using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Services;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

namespace MatchmakingService.Domain.Services;

public class GeneralMatchmakingService : IGeneralMatchmakingService
{
    private readonly ILogger<GeneralMatchmakingService> _logger;
    private readonly IMatchmakingRequestService _matchmakingRequestService;
    private readonly IOwnerMatchmakingService _ownerMatchmakingService;
    private readonly IPlayerDataPoolService _playerDataPoolService;
    private readonly ISystemClock _systemClock;

    public GeneralMatchmakingService(
        IMatchmakingRequestService matchmakingRequestService,
        IPlayerDataPoolService playerDataPoolService,
        IOwnerMatchmakingService ownerMatchmakingService,
        ISystemClock systemClock,
        ILogger<GeneralMatchmakingService> logger)
    {
        _matchmakingRequestService = matchmakingRequestService;
        _playerDataPoolService = playerDataPoolService;
        _ownerMatchmakingService = ownerMatchmakingService;
        _systemClock = systemClock;
        _logger = logger;
    }

    public async Task<GeneralMatchmakingResult> TryMatchmakingAsync(GameType gameType,
        CancellationToken cancellationToken)
    {
        var matchmakingPlayerData = await _playerDataPoolService.PopAsync(gameType, cancellationToken);
        if (matchmakingPlayerData is null)
        {
            _logger.LogWarning("No matchmaking requests found");
            return new GeneralMatchmakingResult {Status = GeneralMatchmakingStatus.Pending};
        }

        if (MatchmakingTimeout(matchmakingPlayerData))
        {
            await _matchmakingRequestService.ClearAsync(matchmakingPlayerData, cancellationToken);
            return new GeneralMatchmakingResult {Status = GeneralMatchmakingStatus.Timeout};
        }

        var ownerMatchmakingResult =
            await _ownerMatchmakingService.TryMatchmakingAsync(matchmakingPlayerData, cancellationToken);

        return ownerMatchmakingResult.Status switch
        {
            OwnerMatchmakingStatus.Success => new GeneralMatchmakingResult
                {Status = GeneralMatchmakingStatus.Success, PlayerIds = ownerMatchmakingResult.PlayerIds},
            OwnerMatchmakingStatus.Fail => new GeneralMatchmakingResult {Status = GeneralMatchmakingStatus.Pending},
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    private bool MatchmakingTimeout(MatchmakingPlayerData matchmakingPlayerData)
    {
        return matchmakingPlayerData.ValidUntil < _systemClock.UtcNow;
    }
}