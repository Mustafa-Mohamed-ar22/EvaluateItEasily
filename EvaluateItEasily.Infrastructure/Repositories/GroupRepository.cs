
namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class GroupRepository : GenericRepository<Group>, IGroupRepository
    {
        public GroupRepository(AppDbContext context) : base(context) { }

        public async Task<Group?> GetWithMembersAsync(int id, CancellationToken ct = default) =>
            await _context.Groups
               .Include(x => x.Leader)
                 .Include(x => x.Members)
                     .ThenInclude(x => x.Student)
                 .Include(x => x.Proposal)
                     .ThenInclude(p => p.SupervisorAssignment)
                         .ThenInclude(sa => sa.Supervisor)
                 .Include(x => x.Proposal)
                     .ThenInclude(p => p.SupervisorAssignment)
                         .ThenInclude(sa => sa.TechnicalAssistant)
                .FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<IEnumerable<Group>> GetAllWithMembersAsync(CancellationToken ct = default) =>
             await _context.Groups
                 .Include(x => x.Leader)
                 .Include(x => x.Members)
                     .ThenInclude(x => x.Student)
                 .Include(x => x.Proposal)
                     .ThenInclude(p => p.SupervisorAssignment)
                         .ThenInclude(sa => sa.Supervisor)
                 .Include(x => x.Proposal)
                     .ThenInclude(p => p.SupervisorAssignment)
                         .ThenInclude(sa => sa.TechnicalAssistant)
                 .OrderByDescending(x => x.CreatedOn)
                 .ToListAsync(ct);

        public async Task<Group?> GetByLeaderIdAsync(string leaderId, CancellationToken ct = default) =>
            await _context.Groups
                .FirstOrDefaultAsync(x => x.LeaderId == leaderId, ct);

        public async Task<Group?> GetByMemberIdAsync(string studentId, CancellationToken ct = default) =>
            await _context.Groups
               .Include(x => x.Leader)
                 .Include(x => x.Members)
                     .ThenInclude(x => x.Student)
                 .Include(x => x.Proposal)
                     .ThenInclude(p => p.SupervisorAssignment)
                         .ThenInclude(sa => sa.Supervisor)
                 .Include(x => x.Proposal)
                     .ThenInclude(p => p.SupervisorAssignment)
                         .ThenInclude(sa => sa.TechnicalAssistant)
                .FirstOrDefaultAsync(x => x.Members.Any(x => x.StudentId == studentId), ct);
        public async Task<Group?> GetByProposalIdAsync(int proposalId, CancellationToken ct = default) =>
            await _context.Groups
               .Include(x => x.Leader)
                 .Include(x => x.Members)
                     .ThenInclude(x => x.Student)
                 .Include(x => x.Proposal)
                     .ThenInclude(p => p.SupervisorAssignment)
                         .ThenInclude(sa => sa.Supervisor)
                 .Include(x => x.Proposal)
                     .ThenInclude(p => p.SupervisorAssignment)
                         .ThenInclude(sa => sa.TechnicalAssistant)
                            .FirstOrDefaultAsync(x => x.Proposal != null && x.Proposal.Id == proposalId, ct);
        public async Task<IEnumerable<string>> GetAllAssignedStudentIdsAsync(CancellationToken ct = default) =>
                await _context.GroupMembers
                .Select(m => m.StudentId)
                .Distinct()
                .ToListAsync(ct);
    }
}