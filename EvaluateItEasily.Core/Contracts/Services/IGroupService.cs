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
        Task<Result<GroupResponse>> AddMemberAsync(int groupId, AddMemberRequest request, CancellationToken ct = default);
        Task<Result> RemoveMemberAsync(int groupId, string studentId, CancellationToken ct = default);
    }
}
