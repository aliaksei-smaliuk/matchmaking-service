namespace MatchmakingService.DataAccess.Kafka.Configurations;

public record KafkaOptions
{
    public string BootstrapServers { get; init; } = null!;
    public string GroupId { get; init; } = null!;
}