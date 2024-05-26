using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Dto.TournamentDtos;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Dto.GameDtos;

namespace TournamentAPI.Data.Data
{
    public class TournamentMappings : Profile
    {
        public TournamentMappings() 
        {
            CreateMap<Tournament, TournamentDto>();
            CreateMap<TournamentDto, Tournament>();

            CreateMap<Tournament, TournamentPostDto>();
            CreateMap<TournamentPostDto, Tournament>();

            CreateMap<Tournament, TournamentWithoutGamesDto>();
            CreateMap<TournamentWithoutGamesDto, Tournament>();

            CreateMap<Game, GameDto>();
            CreateMap<GameDto, Game>();

            CreateMap<Game, GamePostDto>();
            CreateMap<GamePostDto, Game>();

            CreateMap<Game, GameWithIdDto>();
            CreateMap<GameWithIdDto, Game>();
        }
    }
}
