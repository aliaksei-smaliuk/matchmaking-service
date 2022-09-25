namespace MatchmakingService.Domain.Abstraction.Models;

public record GameConfiguration
{
    public int MinPlayersPerRoom { get; set; }
    public int MaxPlayersPerRoom { get; set; }
}