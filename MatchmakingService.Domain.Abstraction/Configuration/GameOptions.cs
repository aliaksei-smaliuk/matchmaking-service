using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Domain.Abstraction.Configuration;

public record GameOptions
{
    public int MaxCashAmount { get; set; }
    public GameConfiguration SmallGameConfiguration { get; set; } = null!;
    public GameConfiguration StandardGameConfiguration { get; set; } = null!;
}