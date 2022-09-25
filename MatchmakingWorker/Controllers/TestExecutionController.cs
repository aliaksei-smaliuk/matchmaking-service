using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchmakingWorker.Controllers;

public class TestExecutionController : ControllerBase
{
    private readonly IGeneralMatchmakingService _generalMatchmakingService;

    public TestExecutionController(IGeneralMatchmakingService generalMatchmakingService)
    {
        _generalMatchmakingService = generalMatchmakingService;
    }

    [HttpPost]
    public async Task<IActionResult> TryMatchmaking([FromQuery] GameType gameType, CancellationToken cancellationToken)
    {
        var matchmakingResult = await _generalMatchmakingService.TryMatchmakingAsync(gameType, cancellationToken);
        return matchmakingResult.Status switch
        {
            GeneralMatchmakingStatus.Success => Ok(matchmakingResult.PlayerIds),
            GeneralMatchmakingStatus.Pending => NotFound(),
            GeneralMatchmakingStatus.Timeout => BadRequest("Matchmaking timeout"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}