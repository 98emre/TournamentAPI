using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Core.Repositories
{
    public interface ITournamentRepository
    {
        Task<(IEnumerable<Tournament>, PaginationMetaData)> GetAllAsync(bool includeGames, bool sort, int pageSize, int pageNumber);
        Task<Tournament> GetAsync(int id, bool includeGames);
        Task<bool> AnyAsync(int id);
        void Add(Tournament tournament);
        void Update(Tournament tournament);
        void Remove(Tournament tournament);
    }
}
