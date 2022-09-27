using MatchmakingService.Contracts.Requests;
using MatchmakingTestClient.Clients;
using Microsoft.AspNetCore.Mvc;

namespace MatchmakingTestClient.Controllers;

[ApiController]
[Route("[controller]")]
public class MatchmakingController : ControllerBase
{
    private readonly IMatchmakingServiceClient _matchmakingServiceClient;

    public MatchmakingController(IMatchmakingServiceClient matchmakingServiceClient)
    {
        _matchmakingServiceClient = matchmakingServiceClient;
    }

    [HttpPost]
    public async Task<IActionResult> AddRequestAsync(AddMatchmakingRequest request, CancellationToken cancellationToken)
    {
        await _matchmakingServiceClient.AddAsync(request, cancellationToken);
        return NoContent();
    }
}