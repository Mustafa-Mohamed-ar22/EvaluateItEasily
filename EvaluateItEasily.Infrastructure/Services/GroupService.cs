using Azure.Core;
using EvaluateItEasily.Core;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s.Groups;
using EvaluateItEasily.Core.Entities;
using EvaluateItEasily.Core.Results;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace EvaluateItEasily.Infrastructure.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GroupService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<Result<IEnumerable<GroupResponse>>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.Groups.GetAllWithMembersAsync(ct);
            return Result.Success(result.Adapt<IEnumerable<GroupResponse>>());
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

            var created = await _unitOfWork.Groups.GetWithMembersAsync(group.Id, ct);
            return Result.Success(created.Adapt<GroupResponse>());
        }

        public async Task<Result<GroupResponse>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var result = await _unitOfWork.Groups.GetWithMembersAsync(id,ct);
            return Result.Success(result.Adapt<GroupResponse>());
        }

        public async Task<Result<GroupResponse>> GetMyGroupAsync(CancellationToken ct = default)
        {
            string CurrentUserId = _currentUserService.GetUserId()!;
            var group = await _unitOfWork.Groups.GetByMemberIdAsync(CurrentUserId, ct);
            if (group is null)
                return Result.Failure<GroupResponse>(GroupErrors.NoGroupFound);

            return Result.Success(group.Adapt<GroupResponse>());
        }

        public async Task<Result> RemoveMemberAsync(int groupId,string studentId,CancellationToken ct = default)
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

            return Result.Success();
        }
    }
}