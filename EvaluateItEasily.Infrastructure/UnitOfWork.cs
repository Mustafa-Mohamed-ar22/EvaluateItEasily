
using EvaluateItEasily.Core.Contracts;

namespace EvaluateItEasily.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGroupRepository Groups { get; private set; }
        public IProposalRepository Proposals { get; private set; }

        public INotificationRepository Notifications { get; private set; }

        public IHistoricalProjectsRepository HistoricalProjects { get; private set; }

        public IEvaluationRepository Evaluations { get; private set; }

        public IGenericRepository<SimilarityResult> SimilarityResults { get; private set; }

        public IDecisionRepository Decisions { get; private set; }

        public IGenericRepository<GroupMember> GroupMembers { get; private set; }

        public ISupervisorAssignmentRepository SupervisorAssignments { get; private set; }

        public UnitOfWork(AppDbContext context, IGroupRepository groupRepository, 
            IProposalRepository proposals, INotificationRepository notifications,
            IHistoricalProjectsRepository historicalProjects, IEvaluationRepository evaluations,
            IGenericRepository<SimilarityResult> similarityResults, IDecisionRepository decisions, IGenericRepository<GroupMember> groupMembers,
            ISupervisorAssignmentRepository supervisorAssignments)
        {
            _context = context;
            Groups = groupRepository;
            Proposals = proposals;
            Notifications = notifications;
            HistoricalProjects = historicalProjects;
            Evaluations = evaluations;
            SimilarityResults = similarityResults;
            Decisions = decisions;
            GroupMembers = groupMembers;
            SupervisorAssignments = supervisorAssignments;
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
