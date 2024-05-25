using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Data.Data
{
    public static class SeedData
    {
        private static Faker faker;

        public static async Task InitAsync(TournamentAPIContext context)
        {

            if (! await context.Tournament.AnyAsync())
            {
                faker = new Faker();

                var tournaments = GenerateTournaments(2);
                await context.Tournament.AddRangeAsync(tournaments);

                var games = GenerateGames(tournaments, 5);
                await context.Game.AddRangeAsync(games);

                await context.SaveChangesAsync();
            }
        }

        private static IEnumerable<Tournament> GenerateTournaments(int count)
        {
            var tournaments = new List<Tournament>();

            for (int i = 0; i < count; i++)
            {
                var tournament = new Tournament
                {
                    Title = faker.Random.Word(),
                    StartDate = faker.Date.Recent()
                };

                tournaments.Add(tournament);
            }

            return tournaments;
        }

        private static IEnumerable<Game> GenerateGames(IEnumerable<Tournament> tournaments, int gamesPerTournament)
        {
            var games = new List<Game>();

            foreach (var tournament in tournaments)
            {
                for (int i = 0; i < gamesPerTournament; i++)
                {
                    var game = new Game
                    {
                        Title = faker.Random.Word(),
                        Time = faker.Date.Recent(),
                        Tournament = tournament
                    };

                    games.Add(game);
                }
            }

            return games;
        }
    }
}

