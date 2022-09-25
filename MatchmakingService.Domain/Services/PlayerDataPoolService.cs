using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using MatchmakingService.Domain.Abstraction.Services;

namespace MatchmakingService.Domain.Services;

public class PlayerDataPoolService : IPlayerDataPoolService
{
    private readonly IPlayerDataRepository _dataRepository;
    private readonly IPlayerDataPoolRepository _poolRepository;

    public PlayerDataPoolService(IPlayerDataPoolRepository poolRepository,
        IPlayerDataRepository dataRepository)
    {
        _poolRepository = poolRepository;
        _dataRepository = dataRepository;
    }

    public async Task<MatchmakingPlayerData?> PopAsync(GameType gameType, CancellationToken cancellationToken)
    {
        var requestId = await _poolRepository.PopAsync(gameType, cancellationToken);
        if (requestId is null)
            return null;

        var matchmakingPlayerData = await _dataRepository.GetAsync(gameType, requestId, cancellationToken);
        return matchmakingPlayerData;
    }

    public async Task PushAsync(MatchmakingPlayerData matchmakingPlayerData, CancellationToken cancellationToken)
    {
        await _poolRepository.PushAsync(matchmakingPlayerData, cancellationToken);
    }
}