namespace MatchmakingService.Domain.Abstraction.Models;

public record GeneralMatchmakingResult
{
    public GeneralMatchmakingStatus Status { get; init; }
    public IReadOnlyCollection<string>? PlayerIds { get; set; }
}