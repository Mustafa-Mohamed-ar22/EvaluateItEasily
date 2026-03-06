using EvaluateItEasily.Core.Contracts;
using EvaluateItEasily.Core.Entities;

namespace EvaluateItEasily.Core
{
    public interface IUnitOfWork
    {
        IGenericRepository<Proposal> Proposals { get; }
        Task<int> complete(CancellationToken cancellationToken = default!);
    }
}
