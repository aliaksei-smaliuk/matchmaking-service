namespace MatchmakingService.Domain.Abstraction.Models;

public record GeneralMatchmakingResult(string? RootPlayerId)
{
    public GeneralMatchmakingStatus Status { get; init; }
    public IReadOnlyCollection<MatchmakingPlayerData>? MatchmakingPlayerDatas { get; set; }
}