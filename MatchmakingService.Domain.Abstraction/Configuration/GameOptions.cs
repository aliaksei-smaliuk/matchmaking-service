namespace MatchmakingService.Domain.Abstraction.Configuration;

public record GameOptions
{
    public int MaxCashAmount { get; set; }
    public TimeSpan MatchmakingTimeout { get; set; }
}