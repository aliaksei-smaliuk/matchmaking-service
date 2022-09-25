using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;

namespace MatchmakingService.Domain.Repositories;

public class GameTypeConfigurationRepository : IGameTypeConfigurationRepository
{
    public GameConfiguration Get(GameType gameType) =>
        gameType switch
        {
            GameType.Small => new GameConfiguration {MinPlayersPerRoom = 3, MaxPlayersPerRoom = 6},
            GameType.Standard => new GameConfiguration {MinPlayersPerRoom = 5, MaxPlayersPerRoom = 10},
            _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
        };
}