using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Services;

public interface IMatchmakingScoreService
{
    double GetScore(MatchmakingPlayerData matchmakingPlayerData);
}