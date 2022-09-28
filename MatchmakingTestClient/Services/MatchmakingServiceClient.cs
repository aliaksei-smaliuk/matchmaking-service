using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MatchmakingService.Contracts.Requests;
using MatchmakingTestClient.Configurations;
using MatchmakingTestClient.Interfaces;
using Microsoft.Extensions.Options;

namespace MatchmakingTestClient.Services;

public class MatchmakingServiceClient : IMatchmakingServiceClient
{
    private readonly HttpClient _httpClient;

    public MatchmakingServiceClient(IOptions<ServicesOptions> servicesOptions)
    {
        _httpClient = new HttpClient {BaseAddress = new Uri(servicesOptions.Value.MatchmakingServiceUrl)};
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task AddAsync(AddMatchmakingRequest request, CancellationToken cancellationToken)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        await _httpClient.PostAsync("/Matchmaking/Add", content, cancellationToken);
    }
}