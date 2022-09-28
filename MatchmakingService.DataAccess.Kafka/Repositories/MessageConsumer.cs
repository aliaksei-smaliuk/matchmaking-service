using Confluent.Kafka;
using MatchmakingService.DataAccess.Kafka.Abstraction.Repositories;
using MatchmakingService.DataAccess.Kafka.Configurations;
using MatchmakingService.Domain.Abstraction.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MatchmakingService.DataAccess.Kafka.Repositories;

public class MessageConsumer : IMessageConsumer
{
    private readonly ConsumerConfig _config;
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(IOptions<KafkaOptions> kafkaOptions, ILogger<MessageConsumer> logger)
    {
        _logger = logger;
        _config = new ConsumerConfig
        {
            BootstrapServers = kafkaOptions.Value.BootstrapServers,
            GroupId = kafkaOptions.Value.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
    }

    public async Task ConsumeAsync<T>(string topic, IMessageProcessor<T> messageProcessor,
        CancellationToken cancellationToken)
    {
        await Task.Factory.StartNew(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using var consumer = new ConsumerBuilder<Ignore, T>(_config).Build();

                    consumer.Subscribe(new[] {topic});

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        var message = consumeResult.Message.Value;
                        if (message is null)
                            continue;

                        try
                        {
                            await messageProcessor.ProcessAsync(message, cancellationToken);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, "Failed to process message");
                        }
                    }

                    consumer.Close();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed to consume");
                }
            }
        }, TaskCreationOptions.LongRunning);
    }
}