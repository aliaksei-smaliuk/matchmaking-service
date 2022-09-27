using MatchmakingService.DataAccess.Kafka.Abstraction.Repositories;
using MatchmakingTestClient.MessageProcessors;

namespace MatchmakingTestClient.BackgroundWorkers;

public class TestSignalRWorker : BackgroundService
{
    private readonly IMessageConsumer _messageConsumer;
    private readonly IRoomCompletedMessageProcessor _roomCompletedMessageProcessor;
    private readonly ITimeoutPlayerMessageProcessor _timeoutPlayerMessageProcessor;

    public TestSignalRWorker(IMessageConsumer messageConsumer,
        IRoomCompletedMessageProcessor roomCompletedMessageProcessor,
        ITimeoutPlayerMessageProcessor timeoutPlayerMessageProcessor)
    {
        _messageConsumer = messageConsumer;
        _roomCompletedMessageProcessor = roomCompletedMessageProcessor;
        _timeoutPlayerMessageProcessor = timeoutPlayerMessageProcessor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.WhenAll(new[]
            {
                _messageConsumer.ConsumeAsync("RoomCompleted", _roomCompletedMessageProcessor, stoppingToken),
                _messageConsumer.ConsumeAsync("TimeoutPlayer", _timeoutPlayerMessageProcessor, stoppingToken),
            }
        );
    }
}