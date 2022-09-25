namespace MatchmakingService.Domain.Abstraction.Models;

public record MatchmakingPlayerData : PlayerData
{
    public MatchmakingPlayerData(PlayerData parent) : base(parent)
    {
    }

    public string RequestId { get; init; } = Guid.NewGuid().ToString();
    public DateTimeOffset ValidUntil { get; init; }
}