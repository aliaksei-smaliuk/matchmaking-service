using MatchmakingService.Domain.Abstraction.Configuration;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using Microsoft.Extensions.Options;

namespace MatchmakingService.Domain.Repositories;

public class GameTypeConfigurationRepository : IGameTypeConfigurationRepository
{
    private readonly IOptions<GameOptions> _gameOptions;

    public GameTypeConfigurationRepository(IOptions<GameOptions> gameOptions)
    {
        _gameOptions = gameOptions;
    }

    public GameConfiguration Get(GameType gameType) =>
        gameType switch
        {
            GameType.Small => _gameOptions.Value.SmallGameConfiguration,
            GameType.Standard => _gameOptions.Value.StandardGameConfiguration,
            _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
        };
}