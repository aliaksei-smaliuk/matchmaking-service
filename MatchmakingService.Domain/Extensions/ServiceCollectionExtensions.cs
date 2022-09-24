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
            .Configure<GameOptions>(configuration)
            .AddSingleton<IPlayerMatchmakingService, PlayerMatchmakingService>();
    }
}