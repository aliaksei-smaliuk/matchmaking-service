using MatchmakingService.Domain.Abstraction.Services;

namespace MatchmakingService.DataAccess.Kafka.Abstraction.Repositories;

public interface IMessageConsumer
{
    Task ConsumeAsync<T>(string topic, IMessageProcessor<T> messageProcessor,
        CancellationToken cancellationToken);
}