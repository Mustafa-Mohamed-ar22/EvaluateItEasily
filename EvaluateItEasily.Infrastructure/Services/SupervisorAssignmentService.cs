using EvaluateItEasily.Core.DTO_s.SupervisorAssignments;
using Microsoft.AspNetCore.Identity;

namespace EvaluateItEasily.Infrastructure.Services
{
    public class SupervisorAssignmentService : ISupervisorAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cacheService;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string AllAssignmentsCacheKey = "supervisor-assignments:all";
        private static string AssignmentCacheKey(int id) =>$"supervisor-assignments:{id}";
        private static string SupervisorAssignmentsCacheKey(string supervisorId) =>$"supervisor-assignments:supervisor:{supervisorId}";

        public SupervisorAssignmentService(IUnitOfWork unitOfWork,ICurrentUserService currentUserService,
            ICacheService cacheService,UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _cacheService = cacheService;
            _userManager = userManager;
        }

        public async Task<Result<IEnumerable<SupervisorAssignmentResponse>>> GetAllAsync(CancellationToken ct = default)
        {
            var cached = await _cacheService.GetAsync<IEnumerable<SupervisorAssignmentResponse>>(
                AllAssignmentsCacheKey, ct);

            if (cached is not null)
                return Result.Success(cached);

            var assignments = await _unitOfWork.SupervisorAssignments.GetAllWithDetailsAsync(ct);
            var response = assignments.Adapt<IEnumerable<SupervisorAssignmentResponse>>().ToList();

            await _cacheService.SetAsync(AllAssignmentsCacheKey, response, ct);

            return Result.Success<IEnumerable<SupervisorAssignmentResponse>>(response);
        }

        public async Task<Result<SupervisorAssignmentResponse>> GetByIdAsync(int id,CancellationToken ct = default)
        {
            var cached = await _cacheService.GetAsync<SupervisorAssignmentResponse>(
                AssignmentCacheKey(id), ct);

            if (cached is not null)
                return Result.Success(cached);

            var assignment = await _unitOfWork.SupervisorAssignments.GetWithDetailsAsync(id, ct);
            if (assignment is null)
                return Result.Failure<SupervisorAssignmentResponse>(SupervisorAssignmentErrors.NotFound);

            var response = assignment.Adapt<SupervisorAssignmentResponse>();

            await _cacheService.SetAsync(AssignmentCacheKey(id), response, ct);

            return Result.Success(response);
        }

        public async Task<Result<IEnumerable<SupervisorAssignmentResponse>>> GetMyAssignmentsAsync(CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();

            var cached = await _cacheService.GetAsync<IEnumerable<SupervisorAssignmentResponse>>(
                SupervisorAssignmentsCacheKey(currentUserId), ct);

            if (cached is not null)
                return Result.Success(cached);

            var assignments = await _unitOfWork.SupervisorAssignments
                .GetBySupervisorIdAsync(currentUserId, ct);

            var response = assignments.Adapt<IEnumerable<SupervisorAssignmentResponse>>().ToList();

            await _cacheService.SetAsync(SupervisorAssignmentsCacheKey(currentUserId), response, ct);

            return Result.Success<IEnumerable<SupervisorAssignmentResponse>>(response);
        }

        public async Task<Result<SupervisorAssignmentResponse>> CreateAsync(CreateSupervisorAssignmentRequest request,CancellationToken ct = default)
        {
            // Proposal must exist
            var proposal = await _unitOfWork.Proposals.GetWithDetailsAsync(request.ProposalId, ct);
            if (proposal is null)
                return Result.Failure<SupervisorAssignmentResponse>(
                    SupervisorAssignmentErrors.ProposalNotFound);

            // Proposal must be Accepted
            if (proposal.Status != ProposalStatus.Accepted)
                return Result.Failure<SupervisorAssignmentResponse>(
                    SupervisorAssignmentErrors.ProposalNotAccepted);

            // Not already assigned
            var existing = await _unitOfWork.SupervisorAssignments
                .GetByProposalIdAsync(request.ProposalId, ct);
            if (existing is not null)
                return Result.Failure<SupervisorAssignmentResponse>(
                    SupervisorAssignmentErrors.AlreadyAssigned);

            // Validate Supervisor
            var supervisor = await _userManager.FindByIdAsync(request.SupervisorId);
            if (supervisor is null)
                return Result.Failure<SupervisorAssignmentResponse>(
                    SupervisorAssignmentErrors.SupervisorNotFound);

            var supervisorRoles = await _userManager.GetRolesAsync(supervisor);
            if (!supervisorRoles.Contains("Supervisor"))
                return Result.Failure<SupervisorAssignmentResponse>(
                    SupervisorAssignmentErrors.InvalidSupervisor);

            // Validate TechnicalAssistant
            var technicalAssistant = await _userManager.FindByIdAsync(request.TechnicalAssistantId);
            if (technicalAssistant is null)
                return Result.Failure<SupervisorAssignmentResponse>(
                    SupervisorAssignmentErrors.TechnicalAssistantNotFound);

            var taRoles = await _userManager.GetRolesAsync(technicalAssistant);
            if (!taRoles.Contains("TechnicalAssistant"))
                return Result.Failure<SupervisorAssignmentResponse>(
                    SupervisorAssignmentErrors.InvalidTechnicalAssistant);

            var currentUserId = _currentUserService.GetUserId();

            // Create assignment
            var assignment = new SupervisorAssignment
            {
                ProposalId = request.ProposalId,
                SupervisorId = request.SupervisorId,
                TechnicalAssistantId = request.TechnicalAssistantId,
                AssignedById = currentUserId!,
                WorkloadNote = request.WorkloadNote,
                AssignedAt = DateTime.UtcNow
            };

            await _unitOfWork.SupervisorAssignments.AddAsync(assignment, ct);

            // Notify Supervisor
            await _unitOfWork.Notifications.AddAsync(new Notification
            {
                UserId = request.SupervisorId,
                Title = "New Project Assigned",
                Message = $"You have been assigned to supervise the project '{proposal.Title}'",
                Type = NotificationType.SupervisorAssigned,
                CreatedAt = DateTime.UtcNow
            }, ct);

            // Notify TechnicalAssistant
            await _unitOfWork.Notifications.AddAsync(new Notification
            {
                UserId = request.TechnicalAssistantId,
                Title = "New Project Assigned",
                Message = $"You have been assigned as technical assistant for the project '{proposal.Title}'",
                Type = NotificationType.SupervisorAssigned,
                CreatedAt = DateTime.UtcNow
            }, ct);

            // Notify all group members
            foreach (var member in proposal.Group.Members)
            {
                await _unitOfWork.Notifications.AddAsync(new Notification
                {
                    UserId = member.StudentId,
                    Title = "Supervisor & Technical Assistant Assigned",
                    Message = $"Dr. {supervisor.FullName} has been assigned as your supervisor and {technicalAssistant.FullName} as technical assistant for your project",
                    Type = NotificationType.SupervisorAssigned,
                    CreatedAt = DateTime.UtcNow
                }, ct);
            }

            await _unitOfWork.complete(ct);

            // Invalidate cache
            await _cacheService.RemoveAsync(AllAssignmentsCacheKey, ct);
            await _cacheService.RemoveAsync(SupervisorAssignmentsCacheKey(request.SupervisorId), ct);
            await _cacheService.RemoveAsync(SupervisorAssignmentsCacheKey(request.TechnicalAssistantId), ct);

            // Load full assignment for response
            var created = await _unitOfWork.SupervisorAssignments.GetWithDetailsAsync(assignment.Id, ct);
            var response = created!.Adapt<SupervisorAssignmentResponse>();

            // Cache new assignment
            await _cacheService.SetAsync(AssignmentCacheKey(assignment.Id), response, ct);

            return Result.Success(response);
        }
    }
}