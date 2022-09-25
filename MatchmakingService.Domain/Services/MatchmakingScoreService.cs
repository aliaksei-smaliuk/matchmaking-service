using MatchmakingService.Domain.Abstraction.Configuration;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Services;
using Microsoft.Extensions.Options;

namespace MatchmakingService.Domain.Services;

public class MatchmakingScoreService : IMatchmakingScoreService
{
    private readonly IOptions<GameOptions> _gameOptions;

    public MatchmakingScoreService(IOptions<GameOptions> gameOptions)
    {
        _gameOptions = gameOptions;
    }

    public double GetScore(MatchmakingPlayerData matchmakingPlayerData)
    {
        var score = matchmakingPlayerData.Level +
                    (double) matchmakingPlayerData.Cash / _gameOptions.Value.MaxCashAmount;
        return score;
    }
}