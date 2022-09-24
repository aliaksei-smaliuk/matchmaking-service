namespace MatchmakingService.Domain.Abstraction.Models;

public record MatchmakingPlayerData : PlayerData
{
    public MatchmakingPlayerData(PlayerData parent) : base(parent)
    {
    }

    public DateTimeOffset ValidUntil { get; init; }
}