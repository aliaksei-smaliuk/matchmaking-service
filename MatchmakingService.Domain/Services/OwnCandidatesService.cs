using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using MatchmakingService.Domain.Abstraction.Services;

namespace MatchmakingService.Domain.Services;

public class OwnCandidatesService : IOwnCandidatesService
{
    private readonly IGameTypeConfigurationRepository _gameTypeConfigurationRepository;
    private readonly IPlayerDataPoolRepository _playerDataPoolRepository;
    private readonly IPlayerScoreRepository _playerScoreRepository;
    private readonly IMatchmakingPlayerDataSynchronizationService _synchronizationService;

    public OwnCandidatesService(
        IMatchmakingPlayerDataSynchronizationService synchronizationService,
        IGameTypeConfigurationRepository gameTypeConfigurationRepository,
        IPlayerScoreRepository playerScoreRepository,
        IPlayerDataPoolRepository playerDataPoolRepository)
    {
        _synchronizationService = synchronizationService;
        _gameTypeConfigurationRepository = gameTypeConfigurationRepository;
        _playerScoreRepository = playerScoreRepository;
        _playerDataPoolRepository = playerDataPoolRepository;
    }

    public async Task<OwnCandidatesResult> OwnCandidatesAsync(
        IReadOnlyCollection<MatchmakingPlayerData> candidatesSample, MatchmakingPlayerData owner,
        CancellationToken cancellationToken)
    {
        var candidate2CanOwnTask =
            candidatesSample
                .ToDictionary(
                    c => c,
                    c => _synchronizationService.CanOwnAsync(c, owner, cancellationToken)
                );

        await Task.WhenAll(candidate2CanOwnTask.Values);

        var gameConfiguration = _gameTypeConfigurationRepository.Get(owner.GameType);

        var roomCandidates =
            candidate2CanOwnTask
                .Where(p => p.Value.Result)
                .Select(p => p.Key)
                .Take(gameConfiguration.MaxPlayersPerRoom)
                .ToList();

        if (roomCandidates.Count < gameConfiguration.MinPlayersPerRoom)
            return new OwnCandidatesResult {IsSuccess = false};

        var finalizationTasks =
            roomCandidates.SelectMany(c =>
            {
                return new[]
                {
                    _synchronizationService.OwnAsync(c, owner, cancellationToken),
                    _playerScoreRepository.RemoveAsync(c, cancellationToken),
                    _playerDataPoolRepository.RemoveAsync(c, cancellationToken),
                };
            });
        await Task.WhenAll(finalizationTasks);

        return new OwnCandidatesResult {IsSuccess = true, OwnedCandidates = roomCandidates.AsReadOnly()};
    }
}