namespace MatchmakingService.Domain.Abstraction.Models;

public record OwnerMatchmakingResult
{
    public static readonly OwnerMatchmakingResult Fail = new(OwnerMatchmakingStatus.Fail, null);

    private OwnerMatchmakingResult(OwnerMatchmakingStatus status, IReadOnlyCollection<string>? playerIds)
    {
        Status = status;
        PlayerIds = playerIds;
    }

    public OwnerMatchmakingStatus Status { get; }
    public IReadOnlyCollection<string>? PlayerIds { get; }

    public static OwnerMatchmakingResult Success(IReadOnlyCollection<string> playerIds) =>
        new(OwnerMatchmakingStatus.Success, playerIds);
}