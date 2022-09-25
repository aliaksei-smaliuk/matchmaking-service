using MatchmakingService.DataAccess.Redis.Configurations;
using MatchmakingService.DataAccess.Redis.Interfaces;
using MatchmakingService.DataAccess.Redis.Repositories;
using MatchmakingService.DataAccess.Redis.Services;
using MatchmakingService.Domain.Abstraction.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MatchmakingService.DataAccess.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedisDataAccess(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        return serviceCollection
            .Configure<RedisOptions>(configuration)
            .AddSingleton<IRedisService, RedisService>()
            .AddSingleton<IOwnerListRepository, OwnerListRepository>()
            .AddSingleton<IPlayerDataPoolRepository, PlayerDataPoolRepository>()
            .AddSingleton<IPlayerScoreRepository, PlayerScoreRepository>()
            .AddSingleton<IPlayerDataRepository, PlayerDataRepository>();
    }
}