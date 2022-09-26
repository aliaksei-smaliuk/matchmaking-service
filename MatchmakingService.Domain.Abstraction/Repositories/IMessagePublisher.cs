namespace MatchmakingService.Domain.Abstraction.Repositories;

public interface IMessagePublisher
{
    Task SendAsync<T>(string topic, T message, CancellationToken cancellationToken);
}