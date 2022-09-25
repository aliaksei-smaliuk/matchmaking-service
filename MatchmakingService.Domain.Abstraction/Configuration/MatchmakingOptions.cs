namespace MatchmakingService.Domain.Abstraction.Configuration;

public class MatchmakingOptions
{
    public TimeSpan MatchmakingTimeout { get; set; }
    public OwnOptions OwnOptions { get; set; } = null!;
}