namespace MatchmakingService.Domain.Abstraction.Models;

public record OwnerMatchmakingResult
{
    public static readonly OwnerMatchmakingResult Fail = new(OwnerMatchmakingStatus.Fail, null);

    private OwnerMatchmakingResult(OwnerMatchmakingStatus status,
        IReadOnlyCollection<MatchmakingPlayerData>? matchmakingPlayerDatas)
    {
        Status = status;
        MatchmakingPlayerDatas = matchmakingPlayerDatas;
    }

    public OwnerMatchmakingStatus Status { get; }
    public IReadOnlyCollection<MatchmakingPlayerData>? MatchmakingPlayerDatas { get; }

    public static OwnerMatchmakingResult Success(IReadOnlyCollection<MatchmakingPlayerData> matchmakingPlayerDatas) =>
        new(OwnerMatchmakingStatus.Success, matchmakingPlayerDatas);
}