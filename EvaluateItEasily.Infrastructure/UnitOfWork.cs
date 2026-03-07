using EvaluateItEasily.Core;
using EvaluateItEasily.Core.Contracts;
using EvaluateItEasily.Core.Contracts.Repositories;
using EvaluateItEasily.Core.Entities;
using EvaluateItEasily.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EvaluateItEasily.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGroupRepository Groups { get; private set; }

        public UnitOfWork(AppDbContext context,IGroupRepository groupRepository)
        {
            _context = context;
            Groups = groupRepository;
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
