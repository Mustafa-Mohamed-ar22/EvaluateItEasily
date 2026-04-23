using EvaluateItEasily.Core.DTO_s.Groups;


namespace EvaluateItEasily.Infrastructure.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICacheService _cacheService;
        private readonly string cacheKey = "AllGroups";
        private const string AvailableStudentsCacheKey = "AvailableStudents";
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
                if(dbResult is null)
                    return Result.Failure<IEnumerable<GroupResponse>>(GroupErrors.NotFound);
                result = dbResult.Adapt<IEnumerable<GroupResponse>>().ToList();
                await _cacheService.SetAsync(cacheKey, result, ct);
            }
            return Result.Success(result);
        }
        //public async Task<Result<GroupResponse>> AddMemberAsync(int groupId, AddMemberRequest request, CancellationToken ct = default)
        //{
        //    var currentUserId = _currentUserService.GetUserId();

        //    var group = await _unitOfWork.Groups.GetByMemberIdAsync(currentUserId!, ct);
        //    if (group is null)
        //        return Result.Failure<GroupResponse>(GroupErrors.NoGroupFound);
        //    if (group.LeaderId != currentUserId)
        //        return Result.Failure<GroupResponse>(GroupErrors.NotLeader);
        //    var student = await _userManager.FindByEmailAsync(request.StudentEmail);
        //    if (student is null)
        //        return Result.Failure<GroupResponse>(GroupErrors.StudentNotFound);

        //    var roles = await _userManager.GetRolesAsync(student);
        //    if (!roles.Contains("Student"))
        //        return Result.Failure<GroupResponse>(GroupErrors.CannotAddNonStudent);

        //    var existingGroup = await _unitOfWork.Groups.GetByMemberIdAsync(student.Id, ct);
        //    if (existingGroup is not null)
        //        return Result.Failure<GroupResponse>(GroupErrors.StudentAlreadyInGroup);

        //    group.Members.Add(new GroupMember
        //    {
        //        StudentId = student.Id,
        //        IsLeader = false,
        //        JoinedAt=DateTime.UtcNow,
        //    });

        //    _unitOfWork.Groups.Update(group);
        //    await _unitOfWork.complete(ct);
        //    await _cacheService.RemoveAsync(cacheKey,ct);
        //    await _cacheService.RemoveAsync($"{cacheKey}-{groupId}", ct);
        //    await _cacheService.RemoveAsync(AvailableStudentsCacheKey, ct);

        //    var updated = await _unitOfWork.Groups.GetWithMembersAsync(groupId, ct);
        //    return Result.Success(updated.Adapt<GroupResponse>());
        //}

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
                LeaderId = currentUserId,
            };

            group.Members.Add(new GroupMember
            {
                StudentId = currentUserId,
                IsLeader = true,
                JoinedAt = DateTime.UtcNow,
            });

            await _unitOfWork.Groups.AddAsync(group, ct);
            await _unitOfWork.complete(ct);
            await _cacheService.RemoveAsync(cacheKey, ct);
            await _cacheService.RemoveAsync(AvailableStudentsCacheKey, ct);

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
                if(Dbresult is null)
                    return Result.Failure<GroupResponse>(GroupErrors.NoGroupFound);
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
            await _cacheService.RemoveAsync(AvailableStudentsCacheKey, ct);

            return Result.Success();
        }

        public async Task<Result<IEnumerable<UserResponse>>> GetAvailableStudentsAsync(CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();

            var cached = await _cacheService.GetAsync<List<UserResponse>>(AvailableStudentsCacheKey, ct);

            if (cached is null)
            {
                var allStudents = await _userManager.GetUsersInRoleAsync(UserRole.Student.ToString());
                var assignedStudentIds = await _unitOfWork.Groups.GetAllAssignedStudentIdsAsync(ct);

                cached = allStudents
                    .Where(s => s.IsActive && !assignedStudentIds.Contains(s.Id))
                    .Select(s => new UserResponse(
                        Id: s.Id,
                        FullName: s.FullName,
                        Email: s.Email!,
                        Role: UserRole.Student.ToString(),
                        IsActive: s.IsActive,
                        CreatedOn: s.CreatedOn
                    ))
                    .ToList();

                await _cacheService.SetAsync(AvailableStudentsCacheKey, cached, ct);
            }

            var result = cached.Where(s => s.Id != currentUserId);

            return Result.Success(result);
        }


        public async Task<Result<GroupInvitationResponse>> SendInvitationAsync(int groupId,AddMemberRequest request,CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();

            var group = await _unitOfWork.Groups.GetWithMembersAsync(groupId, ct);
            if (group is null)
                return Result.Failure<GroupInvitationResponse>(GroupErrors.NotFound);

            // Only leader can invite
            if (group.LeaderId != currentUserId)
                return Result.Failure<GroupInvitationResponse>(GroupErrors.NotLeader);

            // Find student by email
            var student = await _userManager.FindByEmailAsync(request.StudentEmail);
            if (student is null)
                return Result.Failure<GroupInvitationResponse>(GroupErrors.StudentNotFound);

            // Must be a student role
            var roles = await _userManager.GetRolesAsync(student);
            if (!roles.Contains("Student"))
                return Result.Failure<GroupInvitationResponse>(GroupErrors.CannotAddNonStudent);

            // Student must not already be in a group
            var existingGroup = await _unitOfWork.Groups.GetByMemberIdAsync(student.Id, ct);
            if (existingGroup is not null)
                return Result.Failure<GroupInvitationResponse>(GroupErrors.StudentAlreadyInGroup);

            // No duplicate pending invitation for the same group
            var existingInvitation = await _unitOfWork.GroupInvitations.GetPendingByGroupAndStudentAsync(groupId, student.Id, ct);
            if (existingInvitation is not null)
                return Result.Failure<GroupInvitationResponse>(GroupErrors.InvitationAlreadySent);

            // Create invitation
            var invitation = new GroupInvitation
            {
                GroupId = groupId,
                InvitedStudentId = student.Id,
                Status = InvitationStatus.Pending
            };

            await _unitOfWork.GroupInvitations.AddAsync(invitation, ct);

            // Notify student
            await _unitOfWork.Notifications.AddAsync(new Notification
            {
                UserId = student.Id,
                Title = "Group Invitation",
                Message = $"You have been invited to join group '{group.Name}' led by {group.Leader.FullName}. Please accept or reject the invitation.",
                Type = NotificationType.GroupInvitation,
                CreatedAt = DateTime.UtcNow
            }, ct);

            await _unitOfWork.complete(ct);

            var created = await _unitOfWork.GroupInvitations.GetWithDetailsAsync(invitation.Id, ct);
            return Result.Success(MapToInvitationResponse(created!));
        }

        public async Task<Result> AcceptInvitationAsync(int invitationId,CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();

            var invitation = await _unitOfWork.GroupInvitations.GetWithDetailsAsync(invitationId, ct);
            if (invitation is null)
                return Result.Failure(GroupErrors.InvitationNotFound);

            // Only the invited student can accept
            if (invitation.InvitedStudentId != currentUserId)
                return Result.Failure(GroupErrors.NotInvitedStudent);

            // Cannot handle already handled invitation
            if (invitation.Status != InvitationStatus.Pending)
                return Result.Failure(GroupErrors.InvitationAlreadyHandled);

            // Double check student not already in a group
            var existingGroup = await _unitOfWork.Groups.GetByMemberIdAsync(currentUserId, ct);
            if (existingGroup is not null)
                return Result.Failure(GroupErrors.StudentAlreadyInGroup);

            // Accept invitation
            invitation.Status = InvitationStatus.Accepted;
            invitation.RespondedAt = DateTime.UtcNow;
            _unitOfWork.GroupInvitations.Update(invitation);

            // Add to group members
            await _unitOfWork.GroupMembers.AddAsync(new GroupMember
            {
                GroupId = invitation.GroupId,
                StudentId = currentUserId,
                IsLeader = false
            }, ct);

            // Notify leader
            await _unitOfWork.Notifications.AddAsync(new Notification
            {
                UserId = invitation.Group.LeaderId,
                Title = "Invitation Accepted",
                Message = $"{invitation.InvitedStudent.FullName} has accepted your invitation to join '{invitation.Group.Name}'",
                Type = NotificationType.GroupInvitation,
                CreatedAt = DateTime.UtcNow
            }, ct);

            await _unitOfWork.complete(ct);

            await _cacheService.RemoveAsync($"{cacheKey}-{invitation.GroupId}", ct);
            await _cacheService.RemoveAsync(cacheKey, ct);

            return Result.Success();
        }

        public async Task<Result> RejectInvitationAsync(int invitationId,CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();

            var invitation = await _unitOfWork.GroupInvitations.GetWithDetailsAsync(invitationId, ct);
            if (invitation is null)
                return Result.Failure(GroupErrors.InvitationNotFound);

            // Only the invited student can reject
            if (invitation.InvitedStudentId != currentUserId)
                return Result.Failure(GroupErrors.NotInvitedStudent);

            // Cannot handle already handled invitation
            if (invitation.Status != InvitationStatus.Pending)
                return Result.Failure(GroupErrors.InvitationAlreadyHandled);

            // Reject invitation
            invitation.Status = InvitationStatus.Rejected;
            invitation.RespondedAt = DateTime.UtcNow;
            _unitOfWork.GroupInvitations.Update(invitation);

            // Notify leader
            await _unitOfWork.Notifications.AddAsync(new Notification
            {
                UserId = invitation.Group.LeaderId,
                Title = "Invitation Rejected",
                Message = $"{invitation.InvitedStudent.FullName} has rejected your invitation to join '{invitation.Group.Name}'",
                Type = NotificationType.GroupInvitation,
                CreatedAt = DateTime.UtcNow
            }, ct);

            await _unitOfWork.complete(ct);

            return Result.Success();
        }

        public async Task<Result<IEnumerable<GroupInvitationResponse>>> GetGroupInvitationsAsync(int groupId,CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();

            var group = await _unitOfWork.Groups.GetWithMembersAsync(groupId, ct);
            if (group is null)
                return Result.Failure<IEnumerable<GroupInvitationResponse>>(GroupErrors.NotFound);

            // Only leader can see group invitations
            if (group.LeaderId != currentUserId)
                return Result.Failure<IEnumerable<GroupInvitationResponse>>(GroupErrors.NotLeader);

            var invitations = await _unitOfWork.GroupInvitations.GetByGroupIdAsync(groupId, ct);
            return Result.Success(invitations.Select(MapToInvitationResponse));
        }

        public async Task<Result<IEnumerable<GroupInvitationResponse>>> GetMyInvitationsAsync(CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();
            var invitations = await _unitOfWork.GroupInvitations.GetByStudentIdAsync(currentUserId, ct);
            return Result.Success(invitations.Select(MapToInvitationResponse));
        }

        // ── Private helper ────────────────────────────────────────────────
        private static GroupInvitationResponse MapToInvitationResponse(GroupInvitation invitation) => new(
            Id: invitation.Id,
            GroupId: invitation.GroupId,
            GroupName: invitation.Group.Name,
            LeaderName: invitation.Group.Leader.FullName,
            InvitedStudentId: invitation.InvitedStudentId,
            InvitedStudentName: invitation.InvitedStudent.FullName,
            InvitedStudentEmail: invitation.InvitedStudent.Email!,
            Status: invitation.Status.ToString(),
            CreatedOn: invitation.CreatedOn,
            RespondedAt: invitation.RespondedAt
        );
    }
}