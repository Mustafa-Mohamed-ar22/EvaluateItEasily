using Azure.Core;
using EvaluateItEasily.Core;
using EvaluateItEasily.Core.DTO_s.Groups;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace EvaluateItEasily.Infrastructure.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICacheService _cacheService;
        private readonly string cacheKey = "AllGroups"; 
        public GroupService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager,ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _cacheService = cacheService;
        }

        public async Task<Result<IEnumerable<GroupResponse>>> GetAllAsync(CancellationToken ct = default)
        {
            var cachedResults = await _cacheService.GetAsync< IEnumerable<GroupResponse>>(cacheKey,ct);
            IEnumerable<GroupResponse> result = [];
            if (cachedResults is not null)
            {
                result = cachedResults;
            } else
            {
                var dbResult = await _unitOfWork.Groups.GetAllWithMembersAsync(ct);
                result = dbResult.Adapt<IEnumerable<GroupResponse>>();
                await _cacheService.SetAsync(cacheKey, result, ct);
            }
            return Result.Success(result);
        }
        public async Task<Result<GroupResponse>> AddMemberAsync(int groupId, AddMemberRequest request, CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();

            var group = await _unitOfWork.Groups.GetByMemberIdAsync(currentUserId, ct);
            if (group is null)
                return Result.Failure<GroupResponse>(GroupErrors.NoGroupFound);
            if (group.LeaderId != currentUserId)
                return Result.Failure<GroupResponse>(GroupErrors.NotLeader);
            var student = await _userManager.FindByEmailAsync(request.StudentEmail);
            if (student is null)
                return Result.Failure<GroupResponse>(GroupErrors.StudentNotFound);

            var roles = await _userManager.GetRolesAsync(student);
            if (!roles.Contains("Student"))
                return Result.Failure<GroupResponse>(GroupErrors.CannotAddNonStudent);

            var existingGroup = await _unitOfWork.Groups.GetByMemberIdAsync(student.Id, ct);
            if (existingGroup is not null)
                return Result.Failure<GroupResponse>(GroupErrors.StudentAlreadyInGroup);

            group.Members.Add(new GroupMember
            {
                StudentId = student.Id,
                IsLeader = false
            });

            _unitOfWork.Groups.Update(group);
            await _unitOfWork.complete(ct);
            await _cacheService.RemoveAsync(cacheKey,ct);
            await _cacheService.RemoveAsync($"{cacheKey}-{groupId}", ct);
            var updated = await _unitOfWork.Groups.GetWithMembersAsync(groupId, ct);
            return Result.Success(updated.Adapt<GroupResponse>());
        }

        public async Task<Result<GroupResponse>> CreateAsync(CreateGroupRequest request, CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();

            var existingGroup = await _unitOfWork.Groups.GetByLeaderIdAsync(currentUserId, ct);
            if (existingGroup is not null)
                return Result.Failure<GroupResponse>(GroupErrors.AlreadyHasGroup);

            var memberGroup = await _unitOfWork.Groups.GetByMemberIdAsync(currentUserId, ct);
            if (memberGroup is not null)
                return Result.Failure<GroupResponse>(GroupErrors.StudentAlreadyInGroup);

            var group = new Group
            {
                Name = request.Name,
                LeaderId = currentUserId
            };

            group.Members.Add(new GroupMember
            {
                StudentId = currentUserId,
                IsLeader = true
            });

            await _unitOfWork.Groups.AddAsync(group, ct);
            await _unitOfWork.complete(ct);
            await _cacheService.RemoveAsync(cacheKey, ct);

            var created = await _unitOfWork.Groups.GetWithMembersAsync(group.Id, ct);
            return Result.Success(created.Adapt<GroupResponse>());
        }

        public async Task<Result<GroupResponse>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var cachedResult = await _cacheService.GetAsync<GroupResponse>($"{cacheKey}-{id}", ct);
            GroupResponse result;
            if (cachedResult is not null)
                result = cachedResult;
            else
            {
                var Dbresult = await _unitOfWork.Groups.GetWithMembersAsync(id, ct);
                result = Dbresult.Adapt<GroupResponse>();
                await _cacheService.SetAsync($"{cacheKey}-{id}",result, ct);
            }
            return Result.Success(result);
        }
        public async Task<Result<GroupResponse>> GetMyGroupAsync(CancellationToken ct = default)
        {
            string CurrentUserId = _currentUserService.GetUserId()!;
            var group = await _unitOfWork.Groups.GetByMemberIdAsync(CurrentUserId, ct);
            if (group is null)
                return Result.Failure<GroupResponse>(GroupErrors.NoGroupFound);

            return Result.Success(group.Adapt<GroupResponse>());
        }

        public async Task<Result> RemoveMemberAsync(int groupId, string studentId, CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();

            var group = await _unitOfWork.Groups.GetWithMembersAsync(groupId, ct);
            if (group is null)
                return Result.Failure(GroupErrors.NotFound);

            if (group.LeaderId != currentUserId)
                return Result.Failure(GroupErrors.NotLeader);

            if (studentId == group.LeaderId)
                return Result.Failure(GroupErrors.CannotRemoveLeader);

            var member = group.Members.FirstOrDefault(m => m.StudentId == studentId);
            if (member is null)
                return Result.Failure(GroupErrors.MemberNotFound);

            group.Members.Remove(member);
            _unitOfWork.Groups.Update(group);
            await _unitOfWork.complete(ct);
            await _cacheService.RemoveAsync(cacheKey, ct);
            await _cacheService.RemoveAsync($"{cacheKey}-{groupId}", ct);

            return Result.Success();
        }
        public async Task<Result<IEnumerable<UserResponse>>> GetAvailableStudentsAsync(CancellationToken ct = default)
        {
            var allStudents = await _userManager.GetUsersInRoleAsync(UserRole.Student.ToString());

            var assignedStudentIds = await _unitOfWork.Groups.GetAllAssignedStudentIdsAsync(ct);

            var availableStudents = allStudents
                .Where(s => s.IsActive && !assignedStudentIds.Contains(s.Id))
                .Select(s => new UserResponse(
                    Id: s.Id,
                    FullName: s.FullName,
                    Email: s.Email!,
                    Role: UserRole.Student.ToString(),
                    IsActive: s.IsActive,
                    CreatedOn: s.CreatedOn
                ));

            return Result.Success(availableStudents);
        }
    }
}