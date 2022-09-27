using MatchmakingService.Domain.Abstraction.Services;
using MatchmakingTestClient.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MatchmakingTestClient.MessageProcessors;

public interface IRoomCompletedMessageProcessor : IMessageProcessor<string>
{
}

public class RoomCompletedMessageProcessor : IRoomCompletedMessageProcessor
{
    private readonly IHubContext<MatchmakingHub> _hubContext;

    public RoomCompletedMessageProcessor(IHubContext<MatchmakingHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task ProcessAsync(string message, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.All.SendAsync("RoomCompleted", message, cancellationToken);
    }
}