namespace MatchmakingTestClient.Configurations;

public record ServicesOptions
{
    public string MatchmakingServiceUrl { get; init; } = null!;
}