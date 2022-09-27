using System.ComponentModel.DataAnnotations;
using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Contracts.Requests;

public class AddMatchmakingRequest
{
    [Required] public string? PlayerId { get; set; }
    [Required] public int? Level { get; set; }
    [Required] public int? Cash { get; set; }
    [Required] public Platform? Platform { get; set; }
    [Required] public double? HoursInGame { get; set; }
    [Required] public GameType? GameType { get; set; }
}