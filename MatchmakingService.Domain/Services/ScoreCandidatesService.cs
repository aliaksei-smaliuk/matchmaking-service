using MatchmakingService.Domain.Abstraction.Configuration;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using MatchmakingService.Domain.Abstraction.Services;
using Microsoft.Extensions.Options;

namespace MatchmakingService.Domain.Services;

public class ScoreCandidatesService : IScoreCandidatesService
{
    private readonly IOptions<GameOptions> _gameOptions;
    private readonly IOptions<MatchmakingOptions> _matchmakingOptions;
    private readonly IPlayerDataRepository _playerDataRepository;
    private readonly IPlayerScoreRepository _playerScoreRepository;

    public ScoreCandidatesService(
        IPlayerScoreRepository playerScoreRepository,
        IPlayerDataRepository playerDataRepository,
        IOptions<MatchmakingOptions> matchmakingOptions,
        IOptions<GameOptions> gameOptions)
    {
        _playerScoreRepository = playerScoreRepository;
        _playerDataRepository = playerDataRepository;
        _matchmakingOptions = matchmakingOptions;
        _gameOptions = gameOptions;
    }

    public async Task<IReadOnlyCollection<MatchmakingPlayerData>> GetLargeCandidatesSampleAsync(
        MatchmakingPlayerData owner, CancellationToken cancellationToken)
    {
        // TODO Iterate for possible ranges. Skip for MVP purpose.
        var (minLevel, maxLevel) = GetLevelRange(owner);

        var (minCash, maxCash) = GetCashRange(owner);

        var requestsIds = new List<string>();
        for (var i = minLevel; i <= maxLevel; i++)
        {
            var minScore = i + minCash;
            var maxScore = i + maxCash;
            var levelRequestsIds =
                await _playerScoreRepository.GetRangeAsync(owner.GameType, minScore, maxScore, cancellationToken);
            requestsIds.AddRange(levelRequestsIds);
        }

        return await _playerDataRepository.GetAsync(owner.GameType, requestsIds, cancellationToken);
    }

    private (int minLevel, int maxLevel) GetLevelRange(MatchmakingPlayerData owner)
    {
        var minLevel = Math.Max(owner.Level - _matchmakingOptions.Value.MaxRoomLevelDifference / 2, 1);
        var maxLevel = minLevel + _matchmakingOptions.Value.MaxRoomLevelDifference;
        return (minLevel, maxLevel);
    }

    private (int minCash, int maxCash) GetCashRange(MatchmakingPlayerData owner)
    {
        // TODO Use more neat math calculation
        var cashPercentageDispersion = _matchmakingOptions.Value.MaxRoomCashPercentageDifference / 2 / 100;
        var minCash = owner.Cash * (1 - cashPercentageDispersion) / _gameOptions.Value.MaxCashAmount;
        var maxCash = owner.Cash * (1 + cashPercentageDispersion) / _gameOptions.Value.MaxCashAmount;
        return (minCash, maxCash);
    }
}