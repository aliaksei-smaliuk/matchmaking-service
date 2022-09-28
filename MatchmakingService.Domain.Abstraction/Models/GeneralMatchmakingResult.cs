namespace MatchmakingService.Domain.Abstraction.Models;

public record GeneralMatchmakingResult(MatchmakingPlayerData? RootPlayerData)
{
    public GeneralMatchmakingStatus Status { get; init; }
    public IReadOnlyCollection<MatchmakingPlayerData>? MatchmakingPlayerDatas { get; set; }
}