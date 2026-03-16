using EvaluateItEasily.Core.Entities;


namespace EvaluateItEasily.Core.Contracts.Repositories
{
    public interface IProposalRepository : IGenericRepository<Proposal>
    {
        Task<Proposal?> GetWithDetailsAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<Proposal>> GetAllWithDetailsAsync(CancellationToken ct = default);
        Task<Proposal?> GetByGroupIdAsync(int groupId, CancellationToken ct = default);
        Task<IEnumerable<Proposal>> GetAcceptedNotArchivedAsync(CancellationToken ct = default);

    }
}
