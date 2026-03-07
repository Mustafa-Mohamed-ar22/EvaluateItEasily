using EvaluateItEasily.Core.Contracts;
using EvaluateItEasily.Core.Contracts.Repositories;
using EvaluateItEasily.Core.Entities;

namespace EvaluateItEasily.Core
{
    public interface IUnitOfWork
    {
        IGroupRepository Groups { get; }
        Task<int> complete(CancellationToken cancellationToken = default!);
    }
}
