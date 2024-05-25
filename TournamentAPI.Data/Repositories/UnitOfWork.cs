using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TournamentAPIContext _context;

        public UnitOfWork(TournamentAPIContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ITournamentRepository TournamentRepository => new TournamentRepository(_context);

        public IGameRepository GameRepository => new GameRepository(_context);

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
