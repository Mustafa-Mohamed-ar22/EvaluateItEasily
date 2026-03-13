
using EvaluateItEasily.Core.Contracts;

namespace EvaluateItEasily.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGroupRepository Groups { get; private set; }
        public IProposalRepository Proposals { get; private set; }

        public IGenericRepository<Notification> Notifications { get; private set; }

        public UnitOfWork(AppDbContext context, IGroupRepository groupRepository, IProposalRepository proposals, IGenericRepository<Notification> notifications)
        {
            _context = context;
            Groups = groupRepository;
            Proposals = proposals;
            Notifications = notifications;
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
