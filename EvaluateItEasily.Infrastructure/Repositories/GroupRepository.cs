using EvaluateItEasily.Core.Contracts;
using EvaluateItEasily.Core.Contracts.Repositories;
using EvaluateItEasily.Core.Entities;
using EvaluateItEasily.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
    }
}
