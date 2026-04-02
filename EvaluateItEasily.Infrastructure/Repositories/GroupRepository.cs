
namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class GroupRepository : GenericRepository<Group>, IGroupRepository
    {
        public GroupRepository(AppDbContext context) : base(context) { }

        public async Task<Group?> GetWithMembersAsync(int id, CancellationToken ct = default) =>
            await _context.Groups
                .Include(x=>x.Leader)
                .Include(x=>x.Members)
                    .ThenInclude(x=> x.Student)
                .FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<IEnumerable<Group>> GetAllWithMembersAsync(CancellationToken ct = default) =>
            await _context.Groups
                .Include(x=>x.Leader)
                .Include(x=>x.Members)
                    .ThenInclude(x => x.Student)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync(ct);

        public async Task<Group?> GetByLeaderIdAsync(string leaderId, CancellationToken ct = default) =>
            await _context.Groups
                .FirstOrDefaultAsync(x => x.LeaderId == leaderId, ct);

        public async Task<Group?> GetByMemberIdAsync(string studentId, CancellationToken ct = default) =>
            await _context.Groups
                .Include(x=> x.Leader)
                .Include(x=> x.Members)
                    .ThenInclude(x => x.Student)
                .FirstOrDefaultAsync(x => x.Members.Any(x => x.StudentId == studentId), ct);
        public async Task<Group?> GetByProposalIdAsync(int proposalId, CancellationToken ct = default) =>
            await _context.Groups
                .Include(x=>x.Proposal)
                .Include(x => x.Leader)
                .Include(x => x.Members)
                    .ThenInclude(x => x.Student)
                .FirstOrDefaultAsync(x => x.Proposal.Id==proposalId, ct);

        //public async Task<IEnumerable<GroupMember>> GetAvailbleStudents(CancellationToken ct = default)
        //{
        //    var studentRoleId = await _context.Roles.Where(x => x.Name == UserRole.Student.ToString()).Select(x=>x.Id).FirstOrDefaultAsync(ct);

        //    var allStudents =  _context.UserRoles.Where(x => x.RoleId == studentRoleId);

        //    var existedGroupMembers =await _context.GroupMembers.Select(x=>x.StudentId).ToListAsync(ct);

        //    var availableStudents = allStudents.Where(x => existedGroupMembers.Contains(x.UserId));

        //    return availableStudents;
        //}
        public async Task<IEnumerable<string>> GetAllAssignedStudentIdsAsync(CancellationToken ct = default) =>
                await _context.GroupMembers
                .Select(m => m.StudentId)
                .Distinct()
                .ToListAsync(ct);
    }
}