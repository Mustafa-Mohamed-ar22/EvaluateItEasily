using EvaluateItEasily.Core.DTO_s.Evaluations;
using EvaluateItEasily.Core.Settings;
using Microsoft.Extensions.Options;

namespace EvaluateItEasily.Infrastructure.Services
{
    public class EvaluationService(IUnitOfWork unitOfWork, ICacheService cacheService, ICurrentUserService currentUserService, IOptions<AISettings> aiSettings, IAIService aIServive) : IEvaluationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IAIService _AIServive = aIServive;
        private AISettings _aiSettings = aiSettings.Value;
        private static string EvaluationCacheKey(int proposalId) =>$"evaluations:proposal:{proposalId}";
        private static string AllEvaluationCacheKey =$"allevaluations";

        public async Task<Result<EvaluationResponse>> GetByProposalIdAsync(int proposalId,CancellationToken ct = default)
        {
            var currentUserRole = _currentUserService.GetUserRole();

            if (currentUserRole == "Student")
            {
                var currentUserGroup = await _unitOfWork.Groups.GetByProposalIdAsync(proposalId, ct);

                if (currentUserGroup is null)
                    return Result.Failure<EvaluationResponse>(EvaluationError.ProposalNotFound);

                var isMember = currentUserGroup.Members
                    .Any(x => x.StudentId == _currentUserService.GetUserId());

                if (!isMember)
                    return Result.Failure<EvaluationResponse>(EvaluationError.ProposalNotBelongToStudent);
            }

            var cached = await _cacheService.GetAsync<EvaluationResponse>(EvaluationCacheKey(proposalId), ct);

            if (cached is not null)
                return Result.Success(cached);

            var evaluation = await _unitOfWork.Evaluations.GetWithResultsAsync(proposalId, ct);
            if (evaluation is null)
                return Result.Failure<EvaluationResponse>(EvaluationError.NotFound);

            var response = evaluation.Adapt<EvaluationResponse>();

            await _cacheService.SetAsync(EvaluationCacheKey(proposalId), response, ct);

            return Result.Success(response);
        }

        public async Task<Result<EvaluationResponse>> TriggerEvaluationAsync(int proposalId, CancellationToken ct = default)
        {
            var proposal = await _unitOfWork.Proposals.GetWithDetailsAsync(proposalId, ct);
            if (proposal is null)
                return Result.Failure<EvaluationResponse>(EvaluationError.ProposalNotFound);
            var existingEvaluation = await _unitOfWork.Evaluations.GetWithResultsAsync(proposalId, ct);
            if (existingEvaluation is not null)
                return Result.Failure<EvaluationResponse>(EvaluationError.AlreadyEvaluated);

            if (proposal.Status != ProposalStatus.Pending)
                return Result.Failure<EvaluationResponse>(EvaluationError.ProposalNotPending);

           
            var currentUserId = _currentUserService.GetUserId();

            var evaluation = new Evaluation
            {
                ProposalId = proposalId,
                EvaluatedById = currentUserId,
                AIStatus = AIEvaluationStatus.Pending,
                MaxSimilarityScore = 0,
                EvaluatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Evaluations.AddAsync(evaluation, ct);
            await _unitOfWork.complete(ct);

            proposal.Status = ProposalStatus.UnderReview;
            _unitOfWork.Proposals.Update(proposal);

            try
            {
                // besmellah 
                var aiRequest = new AISimilarityRequest(proposal.Abstract, _aiSettings.TopK);
                var aiResponse = await _AIServive.CallAIApiAsync(aiRequest,ct);

                if (aiResponse is null)
                {
                    _unitOfWork.Evaluations.Delete(evaluation);
                    proposal.Status = ProposalStatus.Pending;
                    await _unitOfWork.complete(ct);
                    return Result.Failure<EvaluationResponse>(EvaluationError.AIServiceFailed);
                }

                var rank = 1;
                foreach (var result in aiResponse.Results)
                {
                    await Console.Out.WriteLineAsync(result.ProjectId.ToString());
                    var historicalProject = await _unitOfWork.HistoricalProjects.GetByProjectIdAsync(result.ProjectId-1, ct);

                    if (historicalProject is null) continue;

                    var similarityResult = new SimilarityResult
                    {
                        EvaluationId = evaluation.Id,
                        HistoricalProjectId = historicalProject.Id,
                        SimilarityScore = result.SimilarityScore,
                        Rank = rank++
                    };

                    await _unitOfWork.SimilarityResults.AddAsync(similarityResult, ct);
                }

                evaluation.AIStatus = AIEvaluationStatus.Completed;
                evaluation.MaxSimilarityScore = aiResponse.Results.Max(r => r.SimilarityScore);
                _unitOfWork.Evaluations.Update(evaluation);

                await _unitOfWork.complete(ct);

                // invalidate cashe
                await _cacheService.RemoveAsync($"proposals:{proposalId}", ct);
                await _cacheService.RemoveAsync("proposals:all", ct);
                await _cacheService.RemoveAsync(AllEvaluationCacheKey, ct);

                var created = await _unitOfWork.Evaluations.GetWithResultsAsync(proposalId, ct);
                var response = created!.Adapt<EvaluationResponse>();

                // cashe the evaluation result
                await _cacheService.SetAsync(EvaluationCacheKey(proposalId), response, ct);

                return Result.Success(response);
            }
            catch
            {
                _unitOfWork.Evaluations.Delete(evaluation);
                proposal.Status = ProposalStatus.Pending;
                _unitOfWork.Proposals.Update(proposal);
                await _unitOfWork.complete(ct);
                return Result.Failure<EvaluationResponse>(EvaluationError.AIServiceFailed);
            }
        }
   
        public async Task<Result<IEnumerable<EvaluationResponse>>> GetAllEvaluationsAsync(CancellationToken ct = default)
        {
            var cached = await _cacheService.GetAsync<IEnumerable<EvaluationResponse>>(AllEvaluationCacheKey, ct);
            if (cached is not null)
                return Result.Success(cached);
            var evaluations = await _unitOfWork.Evaluations.GetAllWithResultsAsync(ct);
            var response = evaluations.Adapt<IEnumerable<EvaluationResponse>>();

            await _cacheService.SetAsync(AllEvaluationCacheKey, response, ct);

            return Result.Success(response);
        }
    }
}
