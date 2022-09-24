using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace MatchmakingService.Controllers;

[ApiController]
[Route("[controller]")]
public class RedisTestController : ControllerBase
{
    private readonly IDistributedCache _distributedCache;

    public RedisTestController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    [HttpGet]
    public int GetNextX()
    {
        int.TryParse(_distributedCache.GetString("XValue"), out var val);
        val++;
        _distributedCache.SetString("XValue", val.ToString());
        return val;
    }
}