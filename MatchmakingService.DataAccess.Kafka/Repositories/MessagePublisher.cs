using System.Text.Json;
using Confluent.Kafka;
using MatchmakingService.DataAccess.Kafka.Configurations;
using MatchmakingService.Domain.Abstraction.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MatchmakingService.DataAccess.Kafka.Repositories;

public class MessagePublisher : IMessagePublisher
{
    private readonly ConsumerConfig _config;
    private readonly ILogger<MessagePublisher> _logger;

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

    public async Task SendAsync<T>(string topic, T message, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"BootstrapServers: {_config.BootstrapServers}");
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            var messageStr = JsonSerializer.Serialize(message);
            var deliveryResult =
                await producer.ProduceAsync(topic, new Message<Null, string> {Value = messageStr}, cancellationToken);
            _logger.LogInformation($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
        }
        catch (ProduceException<Null, string> ex)
        {
            _logger.LogError(ex, $"Delivery failed: {ex.Error.Reason}");
        }
    }
}