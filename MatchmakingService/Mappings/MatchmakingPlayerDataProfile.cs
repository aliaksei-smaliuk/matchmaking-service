using AutoMapper;
using MatchmakingService.Domain.Abstraction.Models;
using MatchmakingService.Requests;

namespace MatchmakingService.Mappings;

public class MatchmakingPlayerDataProfile : Profile
{
    public MatchmakingPlayerDataProfile()
    {
        CreateMap<AddMatchmakingRequest, PlayerData>();
    }
}