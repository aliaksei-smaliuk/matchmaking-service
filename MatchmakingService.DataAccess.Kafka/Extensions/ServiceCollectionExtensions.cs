using MatchmakingService.DataAccess.Kafka.Abstraction.Repositories;
using MatchmakingService.DataAccess.Kafka.Configurations;
using MatchmakingService.DataAccess.Kafka.Repositories;
using MatchmakingService.Domain.Abstraction.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MatchmakingService.DataAccess.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaDataAccess(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        return serviceCollection
            .Configure<KafkaOptions>(configuration)
            .AddSingleton<IMessagePublisher, MessagePublisher>()
            .AddSingleton<IMessageConsumer, MessageConsumer>();
    }
}