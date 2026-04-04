using EvaluateItEasily.Core.Entities;


namespace EvaluateItEasily.Core.Contracts.Repositories
{
    public interface ISupervisorAssignmentRepository : IGenericRepository<SupervisorAssignment>
    {
        Task<SupervisorAssignment?> GetWithDetailsAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<SupervisorAssignment>> GetAllWithDetailsAsync(CancellationToken ct = default);
        Task<IEnumerable<SupervisorAssignment>> GetBySupervisorIdAsync(string supervisorId, CancellationToken ct = default);
        Task<SupervisorAssignment?> GetByProposalIdAsync(int proposalId, CancellationToken ct = default);
    }
}
