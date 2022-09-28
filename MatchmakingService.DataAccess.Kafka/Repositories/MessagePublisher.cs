using System.Text.Json;
using Confluent.Kafka;
using MatchmakingService.DataAccess.Kafka.Configurations;
using MatchmakingService.Domain.Abstraction.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MatchmakingService.DataAccess.Kafka.Repositories;

public class MessagePublisher : IMessagePublisher, IDisposable
{
    private readonly ConsumerConfig _config;
    private readonly ILogger<MessagePublisher> _logger;
    private IProducer<Null, string>? _producer;

    public MessagePublisher(IOptions<KafkaOptions> kafkaOptions, ILogger<MessagePublisher> logger)
    {
        _logger = logger;
        _config = new ConsumerConfig
        {
            BootstrapServers = kafkaOptions.Value.BootstrapServers,
            GroupId = kafkaOptions.Value.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }

    public async Task SendAsync<T>(string topic, T message, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"BootstrapServers: {_config.BootstrapServers}");
            _producer ??= new ProducerBuilder<Null, string>(_config).Build();
            var messageStr = JsonSerializer.Serialize(message);
            var deliveryResult = await _producer.ProduceAsync(topic, new Message<Null, string> {Value = messageStr},
                cancellationToken);
            _logger.LogInformation($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
        }
        catch (ProduceException<Null, string> ex)
        {
            _logger.LogError(ex, $"Delivery failed: {ex.Error.Reason}");
            _producer?.Dispose();
            _producer = null;
        }
    }
}