using MatchmakingService.Contracts.Requests;

namespace MatchmakingTestClient.Interfaces;

public interface IMatchmakingServiceClient
{
    Task AddAsync(AddMatchmakingRequest request, CancellationToken cancellationToken);
}