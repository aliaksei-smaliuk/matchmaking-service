using AutoMapper;
using MatchmakingService.Contracts.Requests;
using MatchmakingService.Domain.Abstraction.Models;

namespace MatchmakingService.Mappings;

public class MatchmakingPlayerDataProfile : Profile
{
    public MatchmakingPlayerDataProfile()
    {
        CreateMap<AddMatchmakingRequest, PlayerData>();
    }
}