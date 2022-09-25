namespace MatchmakingService.Domain.Abstraction.Models;

public record PlayerData
{
    public string PlayerId { get; init; } = null!;
    public int Level { get; init; }
    public int Cash { get; init; }
    public Platform Platform { get; init; }
    public double HoursInGame { get; init; }
    public GameType GameType { get; set; }
}