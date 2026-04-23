using EvaluateItEasily.Core.Entities;
namespace EvaluateItEasily.Core.Contracts.Repositories
{
    public interface IGroupInvitationRepository : IGenericRepository<GroupInvitation>
    {
        Task<GroupInvitation?> GetWithDetailsAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<GroupInvitation>> GetByGroupIdAsync(int groupId, CancellationToken ct = default);
        Task<IEnumerable<GroupInvitation>> GetByStudentIdAsync(string studentId, CancellationToken ct = default);
        Task<GroupInvitation?> GetPendingByGroupAndStudentAsync(int groupId, string studentId, CancellationToken ct = default);
    }
}
