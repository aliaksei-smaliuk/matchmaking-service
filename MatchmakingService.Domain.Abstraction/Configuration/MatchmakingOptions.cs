namespace MatchmakingService.Domain.Abstraction.Configuration;

public class MatchmakingOptions
{
    public TimeSpan MatchmakingTimeout { get; set; }
    public int MaxRoomLevelDifference { get; set; }
    public int MaxRoomCashPercentageDifference { get; set; }
    public OwnOptions OwnOptions { get; set; } = null!;
}