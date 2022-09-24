using AutoMapper;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Services;
using MatchmakingService.Requests;
using Microsoft.AspNetCore.Mvc;

namespace MatchmakingService.Controllers;

[ApiController]
[Route("[controller]")]
public class MatchmakingController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPlayerMatchmakingService _playerMatchmakingService;

    public MatchmakingController(IPlayerMatchmakingService playerMatchmakingService, IMapper mapper)
    {
        _playerMatchmakingService = playerMatchmakingService;
        _mapper = mapper;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddAsync(AddMatchmakingRequest request, CancellationToken cancellationToken)
    {
        var matchmakingPlayerData = _mapper.Map<PlayerData>(request);
        await _playerMatchmakingService.AddPlayerToQueueAsync(matchmakingPlayerData, cancellationToken);
        return Accepted();
    }
}