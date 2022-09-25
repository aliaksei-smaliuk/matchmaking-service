namespace MatchmakingService.Domain.Abstraction.Models;

public record OwnCandidatesResult
{
    public bool IsSuccess { get; set; }
    public IReadOnlyCollection<MatchmakingPlayerData>? OwnedCandidates { get; set; }
}