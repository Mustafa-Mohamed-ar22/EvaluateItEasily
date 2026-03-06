using EvaluateItEasily.Core;
using EvaluateItEasily.Core.Contracts;
using EvaluateItEasily.Core.Entities;
using EvaluateItEasily.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluateItEasily.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGenericRepository<Proposal> Proposals { get; private set; }
       // public IBooksRepository Books { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Proposals = new GenericRepository<Proposal>(_context);
            //Books = new BooksRepository(_context);
        }

        public async Task<int> complete(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

       
    }
}
