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
    public class TournamentRepository : ITournamentRepository
    {
        private readonly TournamentAPIContext _context;

        public TournamentRepository(TournamentAPIContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<(IEnumerable<Tournament>, PaginationMetaData)> GetAllAsync(bool includeGames, bool sort, int pageSize, int pageNumber)
        {
            var query = _context.Tournament.AsQueryable();

            if (sort)
            {
                query = query.OrderBy(c => c.StartDate);
            }

            if (includeGames)
            {
                query.Include(t => t.Games);
            }

            var totaltCount = await query.CountAsync();
            var paginationMetaData = new PaginationMetaData(totaltCount, pageSize, pageNumber);

            var collectionToReturn = await query
                .Skip(pageSize * (pageNumber -1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetaData);
        }

        public async Task<Tournament> GetAsync(int id, bool includeGames)
        {
            return includeGames ?  await _context.Tournament.Include(t => t.Games).FirstOrDefaultAsync(t => t.Id == id)
                : await _context.Tournament.FirstOrDefaultAsync(t => t.Id == id);
        }

        public void Add(Tournament tournament) => _context.Add(tournament);

        public async Task<bool> AnyAsync(int id) => await _context.Tournament.AnyAsync(t => t.Id == id);

        public void Remove(Tournament tournament) => _context.Remove(tournament);

        public void Update(Tournament tournament) => _context.Update(tournament);
    }
}
