using MatchmakingTestClient.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MatchmakingTestClient.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigurationController : ControllerBase
{
    private readonly IOptions<ClientOptions> _clientOptions;

    public ConfigurationController(IOptions<ClientOptions> clientOptions)
    {
        _clientOptions = clientOptions;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_clientOptions.Value);
    }
}