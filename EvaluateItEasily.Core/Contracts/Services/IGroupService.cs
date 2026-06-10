using EvaluateItEasily.Core.DTO_s.Groups;
using EvaluateItEasily.Core.Results;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface IGroupService
    {
        Task<Result<IEnumerable<GroupResponse>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<GroupResponse>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Result<GroupResponse>> GetMyGroupAsync(CancellationToken ct = default);
        Task<Result<GroupResponse>> CreateAsync(CreateGroupRequest request, CancellationToken ct = default);
        Task<Result> RemoveMemberAsync(int groupId, string studentId, CancellationToken ct = default);
        Task<Result<IEnumerable<UserResponse>>> GetAvailableStudentsAsync(CancellationToken ct = default);
        Task<Result<GroupInvitationResponse>> SendInvitationAsync(int groupId, AddMemberRequest request, CancellationToken ct = default);
        Task<Result> AcceptInvitationAsync(int invitationId, CancellationToken ct = default);
        Task<Result> RejectInvitationAsync(int invitationId, CancellationToken ct = default);
        Task<Result<IEnumerable<GroupInvitationResponse>>> GetGroupInvitationsAsync(int groupId, CancellationToken ct = default);
        Task<Result<IEnumerable<GroupInvitationResponse>>> GetMyInvitationsAsync(CancellationToken ct = default);
    }
}
