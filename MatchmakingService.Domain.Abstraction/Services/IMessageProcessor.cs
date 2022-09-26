namespace MatchmakingService.Domain.Abstraction.Services;

public interface IMessageProcessor<in T>
{
    Task ProcessAsync(T message, CancellationToken cancellationToken);
}