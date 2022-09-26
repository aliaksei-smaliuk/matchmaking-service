using MatchmakingService.Domain.Abstraction.Repositories;

namespace MatchmakingWorker.BackgroundWorkers;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public Worker(ILogger<Worker> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = $"Worker running at: {DateTimeOffset.Now}";
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await _messagePublisher.SendAsync("test", message, stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}