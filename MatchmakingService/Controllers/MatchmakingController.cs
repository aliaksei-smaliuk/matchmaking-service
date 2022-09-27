using AutoMapper;
using MatchmakingService.Contracts.Requests;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchmakingService.Controllers;

[ApiController]
[Route("[controller]")]
public class MatchmakingController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMatchmakingRequestService _matchmakingRequestService;

    public MatchmakingController(IMatchmakingRequestService matchmakingRequestService, IMapper mapper)
    {
        _matchmakingRequestService = matchmakingRequestService;
        _mapper = mapper;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddAsync(AddMatchmakingRequest request, CancellationToken cancellationToken)
    {
        var matchmakingPlayerData = _mapper.Map<PlayerData>(request);
        await _matchmakingRequestService.InitAsync(matchmakingPlayerData, cancellationToken);
        return Accepted();
    }
}