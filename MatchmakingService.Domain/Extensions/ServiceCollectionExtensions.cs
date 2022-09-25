using MatchmakingService.Domain.Abstraction.Configuration;
using MatchmakingService.Domain.Abstraction.Services;
using MatchmakingService.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MatchmakingService.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainAccess(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        return serviceCollection
            .Configure<GameOptions>(configuration.GetSection("Game"))
            .Configure<MatchmakingOptions>(configuration.GetSection("Matchmaking"))
            .AddSingleton<IGeneralMatchmakingService, GeneralMatchmakingService>()
            .AddSingleton<IMatchmakingScoreService, MatchmakingScoreService>()
            .AddSingleton<IOwnerMatchmakingService, OwnerMatchmakingService>()
            .AddSingleton<IPlayerDataPoolService, PlayerDataPoolService>()
            .AddSingleton<IScoreCandidatesService, ScoreCandidatesService>()
            .AddSingleton<IMatchmakingPlayerDataOwnerListService, MatchmakingPlayerDataOwnerListService>()
            .AddSingleton<IMatchmakingPlayerDataSynchronizationService, MatchmakingPlayerDataSynchronizationService>()
            .AddSingleton<IMatchmakingRequestService, MatchmakingRequestService>();
    }
}