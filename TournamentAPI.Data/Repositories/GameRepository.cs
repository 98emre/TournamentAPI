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

        public async Task<(IEnumerable<Game>, PaginationMetaData)> GetAllAsync(string? filterTitle, bool sort, int pageSize, int pageNumber)
        {
           var collections = _context.Game as IQueryable<Game>;

           if (sort)
           {
                collections = collections.OrderBy(c => c.Time);
           }

           if (!string.IsNullOrEmpty(filterTitle))
           {
                filterTitle = filterTitle.Trim();
                collections = collections.Where(g => g.Title == filterTitle);
           }

            var totaltCount = await collections.CountAsync();
            var paginationMetaData = new PaginationMetaData(totaltCount, pageSize, pageNumber);

            var collectionToReturn = await collections
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetaData);
        }

        public async Task<Game> GetAsync(int id) => await _context.Game.FirstOrDefaultAsync(g => g.Id == id);

        public void Add(Game game) => _context.Add(game);

        public async Task<bool> AnyAsync(int id) => await _context.Game.AnyAsync(g => g.Id == id);

        public void Remove(Game game) => _context.Game.Remove(game);

        public void Update(Game game) => _context.Game.Update(game);
    }
}
