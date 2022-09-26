using MatchmakingTestClient.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MatchmakingTestClient.BackgroundWorkers;

public class TestSignalRWorker : BackgroundService
{
    private readonly IHubContext<ChatHub> _hubContext;

    public TestSignalRWorker(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Test data {DateTime.UtcNow}", stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}