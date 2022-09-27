namespace MatchmakingTestClient.Configurations;

public record ClientOptions
{
    public string HubUrl { get; init; } = null!;
}