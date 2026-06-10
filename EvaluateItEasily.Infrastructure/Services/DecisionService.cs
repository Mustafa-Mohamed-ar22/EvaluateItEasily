using EvaluateItEasily.Core.DTO_s.Decisions;

namespace EvaluateItEasily.Infrastructure.Services
{
    public class DecisionService : IDecisionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cacheService;
        private static string DecisionTypeCacheKey(string decisionType) =>$"decisions:type:{decisionType.ToLower()}";
        private static string DecisionCacheKey(int proposalId) =>$"decisions:proposal:{proposalId}";

        public DecisionService(IUnitOfWork unitOfWork,ICurrentUserService currentUserService,
            ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _cacheService = cacheService;
        }

        public async Task<Result<DecisionResponse>> CreateAsync(int proposalId,
            CreateDecisionRequest request,CancellationToken ct = default)
        {
            var proposal = await _unitOfWork.Proposals.GetWithDetailsAsync(proposalId, ct);
            if (proposal is null)
                return Result.Failure<DecisionResponse>(DecisionErrors.ProposalNotFound);

            var evaluation = await _unitOfWork.Evaluations.GetWithResultsAsync(proposalId, ct);
            if (evaluation is null || evaluation.AIStatus != AIEvaluationStatus.Completed)
                return Result.Failure<DecisionResponse>(DecisionErrors.ProposalNotEvaluated);

            var existingDecision = await _unitOfWork.Decisions.GetByProposalIdAsync(proposalId, ct);
            if (existingDecision is not null)
                return Result.Failure<DecisionResponse>(DecisionErrors.AlreadyDecided);

            if (!Enum.TryParse<DecisionType>(request.DecisionType, out var decisionType))
                return Result.Failure<DecisionResponse>(DecisionErrors.InvalidDecisionType);

            var currentUserId = _currentUserService.GetUserId();

            var decision = new Decision
            {
                ProposalId = proposalId,
                DecidedById = currentUserId,
                DecisionType = decisionType,
                FeedbackComment = request.FeedbackComment,
                DecidedAt = DateTime.UtcNow
            };

            await _unitOfWork.Decisions.AddAsync(decision, ct);

            proposal.Status = decisionType switch
            {
                DecisionType.Accepted => ProposalStatus.Accepted,
                DecisionType.Rejected => ProposalStatus.Rejected,
                DecisionType.RevisionRequested => ProposalStatus.RevisionRequested,
                _ => proposal.Status
            };

            _unitOfWork.Proposals.Update(proposal);

            var notificationMessage = decisionType switch
            {
                DecisionType.Accepted => 
                $"Congratulations! Your proposal '{proposal.Title}' has been accepted.",
                DecisionType.Rejected => 
                $"Your proposal '{proposal.Title}' has been rejected. Feedback: {request.FeedbackComment}",
                DecisionType.RevisionRequested => 
                $"Your proposal '{proposal.Title}' requires revisions. Feedback: {request.FeedbackComment}",
                _ => string.Empty
            };

            foreach (var member in proposal.Group.Members)
            {
                await _unitOfWork.Notifications.AddAsync(new Notification
                {
                    UserId = member.StudentId,
                    Title = $"Proposal {decisionType}",
                    Message = notificationMessage,
                    Type = NotificationType.DecisionMade,
                    CreatedAt = DateTime.UtcNow
                }, ct);
            }

            await _unitOfWork.complete(ct);

            await InvalidateCashe(proposalId, proposal, ct);
            var created = await _unitOfWork.Decisions.GetByProposalIdAsync(proposalId, ct);
            var response = created!.Adapt<DecisionResponse>();

            await _cacheService.SetAsync(DecisionCacheKey(proposalId), response, ct);

            return Result.Success(response);
        }

        private async Task InvalidateCashe(int proposalId, Proposal? proposal, CancellationToken ct)
        {
            await _cacheService.RemoveAsync($"proposals:{proposalId}", ct);
            await _cacheService.RemoveAsync("proposals:all", ct);
            await _cacheService.RemoveAsync($"proposals:group:{proposal.GroupId}", ct);
            await _cacheService.RemoveAsync(DecisionTypeCacheKey("Accepted"), ct);
            await _cacheService.RemoveAsync(DecisionTypeCacheKey("Rejected"), ct);
            await _cacheService.RemoveAsync(DecisionTypeCacheKey("RevisionRequested"), ct);
        }

        public async Task<Result<DecisionResponse>> GetByProposalIdAsync(int proposalId,CancellationToken ct = default)
        {
            var cached = await _cacheService.GetAsync<DecisionResponse>(DecisionCacheKey(proposalId), ct);

            if (cached is not null)
                return Result.Success(cached);

            var decision = await _unitOfWork.Decisions.GetByProposalIdAsync(proposalId, ct);
            if (decision is null)
                return Result.Failure<DecisionResponse>(DecisionErrors.NotFound);

            var response = decision.Adapt<DecisionResponse>();

            await _cacheService.SetAsync(DecisionCacheKey(proposalId), response, ct);

            return Result.Success(response);
        }
        public async Task<Result<IEnumerable<DecisionResponse>>> GetByDecisionTypeAsync
            (string decisionType,CancellationToken ct = default)
        {
            if (!Enum.TryParse<DecisionType>(decisionType, out var parsedType))
                return Result.Failure<IEnumerable<DecisionResponse>>(DecisionErrors.InvalidDecisionType);

            var cached = await _cacheService.GetAsync<IEnumerable<DecisionResponse>>(
                DecisionTypeCacheKey(decisionType), ct);

            if (cached is not null)
                return Result.Success(cached);

            var decisions = await _unitOfWork.Decisions.GetByDecisionTypeAsync(parsedType, ct);
            var response = decisions.Adapt<IEnumerable<DecisionResponse>>().ToList();

            await _cacheService.SetAsync(DecisionTypeCacheKey(decisionType), response, ct);

            return Result.Success<IEnumerable<DecisionResponse>>(response);
        }
    }
}