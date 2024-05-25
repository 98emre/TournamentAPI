using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Dto;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Data.Data
{
    public class TournamentMappings : Profile
    {
        public TournamentMappings() 
        {
            CreateMap<Tournament, TournamentDto>();
            CreateMap<Game, GameDto>();
            CreateMap<TournamentDto, Tournament>();
            CreateMap<GameDto, Game>();
        }
    }
}
