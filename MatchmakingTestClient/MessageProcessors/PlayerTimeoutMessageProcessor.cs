using MatchmakingService.Domain.Abstraction.Services;
using MatchmakingTestClient.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MatchmakingTestClient.MessageProcessors;

public interface ITimeoutPlayerMessageProcessor : IMessageProcessor<string>
{
}

public class TimeoutPlayerMessageProcessor : ITimeoutPlayerMessageProcessor
{
    private readonly IHubContext<ChatHub> _hubContext;

    public TimeoutPlayerMessageProcessor(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task ProcessAsync(string message, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.All.SendAsync("TimeoutPlayer", message, cancellationToken);
    }
}