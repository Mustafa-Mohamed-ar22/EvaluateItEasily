namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class GroupInvitationRepository : GenericRepository<GroupInvitation>, IGroupInvitationRepository
    {
        public GroupInvitationRepository(AppDbContext context) : base(context) { }

        public async Task<GroupInvitation?> GetWithDetailsAsync(int id, CancellationToken ct = default) =>
            await _context.GroupInvitations
                .Include(gi => gi.Group)
                    .ThenInclude(g => g.Leader)
                .Include(gi => gi.InvitedStudent)
                .FirstOrDefaultAsync(gi => gi.Id == id, ct);

        public async Task<IEnumerable<GroupInvitation>> GetByGroupIdAsync(
            int groupId,
            CancellationToken ct = default) =>
            await _context.GroupInvitations
                .Include(gi => gi.Group)
                    .ThenInclude(g => g.Leader)
                .Include(gi => gi.InvitedStudent)
                .Where(gi => gi.GroupId == groupId)
                .OrderByDescending(gi => gi.CreatedOn)
                .ToListAsync(ct);

        public async Task<IEnumerable<GroupInvitation>> GetByStudentIdAsync(
            string studentId,
            CancellationToken ct = default) =>
            await _context.GroupInvitations
                .Include(gi => gi.Group)
                    .ThenInclude(g => g.Leader)
                .Include(gi => gi.InvitedStudent)
                .Where(gi => gi.InvitedStudentId == studentId)
                .OrderByDescending(gi => gi.CreatedOn)
                .ToListAsync(ct);

        public async Task<GroupInvitation?> GetPendingByGroupAndStudentAsync(
            int groupId,
            string studentId,
            CancellationToken ct = default) =>
            await _context.GroupInvitations
                .FirstOrDefaultAsync(gi => gi.GroupId == groupId
                    && gi.InvitedStudentId == studentId
                    && gi.Status == InvitationStatus.Pending, ct);
    }
}
