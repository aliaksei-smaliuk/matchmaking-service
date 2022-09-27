using MatchmakingService.Contracts.Requests;
using Refit;

namespace MatchmakingTestClient.Clients;

public interface IMatchmakingServiceClient
{
    [Post("/matchmaking/add")]
    Task AddAsync(AddMatchmakingRequest request, CancellationToken cancellationToken);
}