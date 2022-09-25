namespace MatchmakingService.Domain.Abstraction.Configuration;

public class OwnOptions
{
    public TimeSpan OwnTimeout { get; set; }
    public int OwnRetriesCount { get; set; }
}