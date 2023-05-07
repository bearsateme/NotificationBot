using AutoMapper;
using Models.Dtos;
using Models.Entities;

namespace DataAccess.Profiles
{
    public class GuildTeamProfile : Profile
    {
        public GuildTeamProfile()
        {
            CreateMap<GuildTeamEntity, GuildTeamDto>();
        }
    }
}

