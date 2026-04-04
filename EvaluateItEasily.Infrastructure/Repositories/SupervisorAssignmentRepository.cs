namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class SupervisorAssignmentRepository : GenericRepository<SupervisorAssignment>, ISupervisorAssignmentRepository
    {
        public SupervisorAssignmentRepository(AppDbContext context) : base(context) { }

        public async Task<SupervisorAssignment?> GetWithDetailsAsync(int id, CancellationToken ct = default) =>
            await _context.SupervisorAssignments
                .Include(sa => sa.Proposal)
                    .ThenInclude(p => p.Group)
                .Include(sa => sa.Supervisor)
                .Include(sa => sa.AssignedByUser)
                .FirstOrDefaultAsync(sa => sa.Id == id, ct);

        public async Task<IEnumerable<SupervisorAssignment>> GetAllWithDetailsAsync(CancellationToken ct = default) =>
            await _context.SupervisorAssignments
                .Include(sa => sa.Proposal)
                    .ThenInclude(p => p.Group)
                .Include(sa => sa.Supervisor)
                .Include(sa => sa.AssignedByUser)
                .OrderByDescending(sa => sa.AssignedAt)
                .ToListAsync(ct);

        public async Task<IEnumerable<SupervisorAssignment>> GetBySupervisorIdAsync(string supervisorId,CancellationToken ct = default) =>
            await _context.SupervisorAssignments
                .Include(sa => sa.Proposal)
                    .ThenInclude(p => p.Group)
                .Include(sa => sa.Supervisor)
                .Include(sa => sa.AssignedByUser)
                .Where(sa => sa.SupervisorId == supervisorId)
                .OrderByDescending(sa => sa.AssignedAt)
                .ToListAsync(ct);

        public async Task<SupervisorAssignment?> GetByProposalIdAsync(int proposalId, CancellationToken ct = default) =>
            await _context.SupervisorAssignments
                .Include(sa => sa.Proposal)
                    .ThenInclude(p => p.Group)
                .Include(sa => sa.Supervisor)
                .Include(sa => sa.AssignedByUser)
                .FirstOrDefaultAsync(sa => sa.ProposalId == proposalId, ct);
    }
}
