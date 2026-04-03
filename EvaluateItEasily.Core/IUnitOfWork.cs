using EvaluateItEasily.Core.Contracts;
using EvaluateItEasily.Core.Contracts.Repositories;
using EvaluateItEasily.Core.Entities;

namespace EvaluateItEasily.Core
{
    public interface IUnitOfWork
    {
        IGroupRepository Groups { get; }
        IProposalRepository Proposals { get; }
        IGenericRepository<GroupMember> GroupMembers { get; }

        INotificationRepository Notifications { get; }
        IGenericRepository<SimilarityResult> SimilarityResults { get; }
        IDecisionRepository Decisions { get; }
        IEvaluationRepository Evaluations { get; }
        IHistoricalProjectsRepository HistoricalProjects { get; }
        Task<int> complete(CancellationToken cancellationToken = default!);
    }
}
