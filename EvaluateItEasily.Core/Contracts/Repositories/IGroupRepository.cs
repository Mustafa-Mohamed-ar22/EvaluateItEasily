using EvaluateItEasily.Core.Entities;


namespace EvaluateItEasily.Core.Contracts.Repositories
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        Task<IEnumerable<Group>> GetAllWithMembersAsync(CancellationToken cancellationToken = default!);
        Task<Group?> GetWithMembersAsync(int id,CancellationToken cancellationToken = default!);
        public Task<Group?> GetByMemberIdAsync(string studentId, CancellationToken ct = default);
        public Task<Group?> GetByLeaderIdAsync(string leaderId, CancellationToken ct = default);
        Task<IEnumerable<string>> GetAllAssignedStudentIdsAsync(CancellationToken ct = default);
        public Task<Group?> GetByProposalIdAsync(int proposalId, CancellationToken ct = default);
    }
}
