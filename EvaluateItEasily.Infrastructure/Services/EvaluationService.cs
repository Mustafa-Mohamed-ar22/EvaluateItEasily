using EvaluateItEasily.Core.DTO_s.Evaluations;
using EvaluateItEasily.Core.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace EvaluateItEasily.Infrastructure.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrentUserService _currentUserService;
        private AISettings _aiSettings;
        private static string EvaluationCacheKey(int proposalId) =>$"evaluations:proposal:{proposalId}";
        private static string AllEvaluationCacheKey =$"allevaluations";
        public EvaluationService(IUnitOfWork unitOfWork, ICacheService cacheService, IHttpClientFactory httpClientFactory, ICurrentUserService currentUserService, IOptions<AISettings> aiSettings)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _httpClientFactory = httpClientFactory;
            _currentUserService = currentUserService;
            _aiSettings = aiSettings.Value;
        }

        public async Task<Result<EvaluationResponse>> GetByProposalIdAsync(int proposalId, CancellationToken ct = default)
        {
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

            // update proposal status to UnderReview
            proposal.Status = ProposalStatus.UnderReview;
            _unitOfWork.Proposals.Update(proposal);

            try
            {
                // Call Python AI API
                var aiRequest = new AISimilarityRequest(proposal.Abstract, _aiSettings.TopK);
                var aiResponse = await CallAIApiAsync(aiRequest, ct);

                if (aiResponse is null)
                {
                    // Mark evaluation as failed
                    evaluation.AIStatus = AIEvaluationStatus.Failed;
                    _unitOfWork.Evaluations.Update(evaluation);
                    await _unitOfWork.complete(ct);
                    return Result.Failure<EvaluationResponse>(EvaluationError.AIServiceFailed);
                }

                var rank = 1;
                foreach (var result in aiResponse.Results)
                {
                    await Console.Out.WriteLineAsync(result.ProjectId.ToString());
                    // Find matching HistoricalProject by its index from AI API
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

                // update evaluation with final status
                evaluation.AIStatus = AIEvaluationStatus.Completed;
                evaluation.MaxSimilarityScore = aiResponse.Results.Max(r => r.SimilarityScore);
                _unitOfWork.Evaluations.Update(evaluation);

                await _unitOfWork.complete(ct);

                // Invalidate cashe
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
                evaluation.AIStatus = AIEvaluationStatus.Failed;
                _unitOfWork.Evaluations.Update(evaluation);
                await _unitOfWork.complete(ct);
                return Result.Failure<EvaluationResponse>(EvaluationError.AIServiceFailed);
            }
        }
        private async Task<AISimilarityResponse?> CallAIApiAsync(AISimilarityRequest request,CancellationToken ct)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AI_API");
                var response = await client.PostAsJsonAsync($"api/similarity",request,ct);

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<AISimilarityResponse>(cancellationToken: ct);
            }
            catch
            {
                return null;
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
