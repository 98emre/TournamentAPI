using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly TournamentAPIContext _context;

        public GameRepository(TournamentAPIContext context)
        {
            _context = context;
        }

        public void Add(Game game) => _context.Add(game);

        public async Task<bool> AnyAsync(int id) => await _context.Game.AnyAsync(g => g.Id == id);

        public async Task<IEnumerable<Game>> GetAllAsync() => await _context.Game.ToListAsync();

        public async Task<Game> GetAsync(int id) => await _context.Game.FirstOrDefaultAsync(g => g.Id == id);

        public void Remove(Game game) => _context.Game.Remove(game);

        public void Update(Game game) => _context.Game.Update(game);
    }
}
