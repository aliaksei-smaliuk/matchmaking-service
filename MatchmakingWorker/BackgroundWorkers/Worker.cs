using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Domain.Abstraction.Repositories;
using MatchmakingService.Domain.Abstraction.Services;

namespace MatchmakingWorker.BackgroundWorkers;

public class Worker : BackgroundService
{
    private readonly IGeneralMatchmakingService _generalMatchmakingService;
    private readonly ILogger<Worker> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public Worker(IGeneralMatchmakingService generalMatchmakingService, ILogger<Worker> logger,
        IMessagePublisher messagePublisher)
    {
        _generalMatchmakingService = generalMatchmakingService;
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessGameTypeAsync(GameType.Small, stoppingToken);
            await ProcessGameTypeAsync(GameType.Standard, stoppingToken);
            await Task.Delay(100, stoppingToken);
        }
    }

    private async Task ProcessGameTypeAsync(GameType gameType, CancellationToken cancellationToken)
    {
        var generalMatchmakingResult =
            await _generalMatchmakingService.TryMatchmakingAsync(gameType, cancellationToken);
        switch (generalMatchmakingResult.Status)
        {
            case GeneralMatchmakingStatus.Pending:
                break;
            case GeneralMatchmakingStatus.Success:
                await _messagePublisher.SendAsync("RoomCompleted", generalMatchmakingResult.MatchmakingPlayerDatas,
                    cancellationToken);
                _logger.LogInformation($"RoomCompleted {generalMatchmakingResult.RootPlayerId}");
                break;
            case GeneralMatchmakingStatus.Timeout:
                await _messagePublisher.SendAsync("TimeoutPlayer", generalMatchmakingResult.RootPlayerId,
                    cancellationToken);
                _logger.LogWarning($"TimeoutPlayer {generalMatchmakingResult.RootPlayerId}");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}