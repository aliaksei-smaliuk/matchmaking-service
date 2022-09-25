using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Repositories;

public interface IGameTypeConfigurationRepository
{
    public GameConfiguration Get(GameType gameType);
}