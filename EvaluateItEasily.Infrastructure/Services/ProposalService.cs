using EvaluateItEasily.Core.DTO_s.Proposals;
using EvaluateItEasily.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
namespace EvaluateItEasily.Infrastructure.Services
{
    public class ProposalService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService,
        IFileService fileService, 
        ICacheService cacheService, ILogger<ProposalService> logger,
        ISubmissionPeriodService submissionPeriodService,IOptions<SupabaseSettings> filesettings,
        IEvaluationService evaluationService) : IProposalService
    {
        private readonly SupabaseSettings _b2Settings = filesettings.Value;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IFileService _fileService = fileService;
        private readonly ICacheService _cacheService = cacheService;
        private readonly ISubmissionPeriodService _submissionPeriodService = submissionPeriodService;
        private readonly IEvaluationService _evaluationService = evaluationService;

        private static string ProposalCacheKey(int id) => $"proposals:{id}";
        private static string GroupProposalCacheKey(int groupId) => $"proposals:group:{groupId}";
        private static string AllProposalsCacheKey(string? status) =>    string.IsNullOrEmpty(status)?
            "proposals:all": $"proposals:status:{status.ToLower()}";
        private static string ProposalDownloadMetadataCacheKey(int id) =>$"proposals:download-metadata:{id}";
        public async Task<Result<IEnumerable<ProposalResponse>>> GetAllAsync(string? status = null, 
            CancellationToken ct = default)
        {
            if (!string.IsNullOrEmpty(status) &&!Enum.TryParse<ProposalStatus>(status, ignoreCase: true, out _))
                return Result.Failure<IEnumerable<ProposalResponse>>(ProposalErrors.InvalidStatus);

            var cacheKey = AllProposalsCacheKey(status);

            var cached = await _cacheService.GetAsync<IEnumerable<ProposalResponse>>(cacheKey, ct);
            if (cached is not null)
                return Result.Success(cached);

            var proposals = await _unitOfWork.Proposals.GetAllWithDetailsAsync(status, ct);
            var response = proposals.Select(MapToResponse).ToList();

            await _cacheService.SetAsync(cacheKey, response, ct);

            return Result.Success<IEnumerable<ProposalResponse>>(response);
        }

        public async Task<Result<ProposalResponse>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var cached = await _cacheService.GetAsync<ProposalResponse>(ProposalCacheKey(id), ct);
            if (cached is not null)
            {
                logger.LogError("I Entered to the Cached Results for Id ");
                 
                return Result.Success(cached);
            }
            var proposal = await _unitOfWork.Proposals.GetWithDetailsAsync(id, ct);
            if (proposal is null)
                return Result.Failure<ProposalResponse>(ProposalErrors.NotFound);
            var response = MapToResponse(proposal);
            await _cacheService.SetAsync(ProposalCacheKey(id), response, ct);
            return Result.Success(response);
        }
        public async Task<Result<ProposalResponse>> GetMyProposalAsync(CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();

            var group = await _unitOfWork.Groups.GetByMemberIdAsync(currentUserId, ct);
            if (group is null)
                return Result.Failure<ProposalResponse>(ProposalErrors.NoGroup);

            var cached = await _cacheService.GetAsync<ProposalResponse>(GroupProposalCacheKey(group.Id), ct);
            if (cached is not null)
            {
                logger.LogError("I Entered to the Cached Results for user ");

                return Result.Success(cached);
            }

            var proposal = await _unitOfWork.Proposals.GetByGroupIdAsync(group.Id, ct);
            if (proposal is null)
                return Result.Failure<ProposalResponse>(ProposalErrors.NoProposal);

            var response = MapToResponse(proposal);

            await _cacheService.SetAsync(GroupProposalCacheKey(group.Id), response, ct);

            return Result.Success(response);
        }
        public async Task<Result<ProposalResponse>> CreateAsync(CreateProposalRequest request,CancellationToken ct = default)
        {
            var periodCheck = await _submissionPeriodService.ValidateIsOpenAsync(ct);
            if (periodCheck.IsFailure)
                return Result.Failure<ProposalResponse>(periodCheck.Error);


            var currentUserId = _currentUserService.GetUserId();

            var group = await _unitOfWork.Groups.GetByMemberIdAsync(currentUserId, ct);
            if (group is null)
                return Result.Failure<ProposalResponse>(ProposalErrors.NoGroup);

            if (group.LeaderId != currentUserId)
                return Result.Failure<ProposalResponse>(ProposalErrors.NotLeader);

            var existing = await _unitOfWork.Proposals.GetByGroupIdAsync(group.Id, ct);
            if (existing is not null)
                return Result.Failure<ProposalResponse>(ProposalErrors.AlreadySubmitted);

            try
            {
                // Upload file to Supabase through API server
                var urlResult = await _fileService.GenerateDownloadUrlAsync(request.StoredFileName, ct);
                if (urlResult.IsFailure)
                    return Result.Failure<ProposalResponse>(urlResult.Error);

                var proposal = new Proposal
                {
                    FileName = request.OriginalFileName,
                    ContentType = request.ContentType,
                    StoredFileName = request.StoredFileName,
                    FileExtension = Path.GetExtension(request.OriginalFileName),
                    Abstract = CleanText(request.Abstract),
                    Title = request.Title,
                    SubmittedAt = DateTime.UtcNow,
                    Status = ProposalStatus.Pending,
                    GroupId = group.Id,
                    ProposalFileUrl = urlResult.Data,
                    Domain = request.Domain
                };

                await _unitOfWork.Proposals.AddAsync(proposal, ct);
                await NotifyMembers(group, proposal, ct);
                await _unitOfWork.complete(ct);

                var savedProposal = await _unitOfWork.Proposals.GetWithDetailsAsync(proposal.Id, ct);

                var evaluationResult = await _evaluationService.RunAutoEvaluationAsync(savedProposal!, ct);

                if (evaluationResult.IsFailure)
                {
                    // no thing
                }

                var cacheKeys = Enum.GetNames<ProposalStatus>()
                    .Select(s => _cacheService.RemoveAsync(AllProposalsCacheKey(s), ct))
                    .Append(_cacheService.RemoveAsync(AllProposalsCacheKey(null), ct))
                    .Append(_cacheService.RemoveAsync(GroupProposalCacheKey(group.Id), ct))
                    .Append(_cacheService.RemoveAsync("AllGroups", ct));

                await Task.WhenAll(cacheKeys);
                var created = await _unitOfWork.Proposals.GetWithDetailsAsync(proposal.Id, ct);
                var response = MapToResponse(created!);

                await _cacheService.SetAsync(ProposalCacheKey(proposal.Id), response, ct);

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"S3 Error: {ex.Message}");
                Console.WriteLine($"Inner: {ex.InnerException?.Message}");
                return Result.Failure<ProposalResponse>(ProposalErrors.CannotUpload);
            }
        }
        public async Task<Result<ProposalResponse>> UpdateAsync(int id, UpdateProposalRequest request, CancellationToken ct = default)
        {
            var periodCheck = await _submissionPeriodService.ValidateIsOpenAsync(ct);
            if (periodCheck.IsFailure)
                return Result.Failure<ProposalResponse>(periodCheck.Error);

            var currentUserId = _currentUserService.GetUserId();
            var proposal = await _unitOfWork.Proposals.GetWithDetailsAsync(id, ct);

            if (proposal is null)
                return Result.Failure<ProposalResponse>(ProposalErrors.NotFound);
            if (proposal.Group.LeaderId != currentUserId)
                return Result.Failure<ProposalResponse>(ProposalErrors.NotLeader);
            if (!(proposal.Status == ProposalStatus.Pending ||
                  proposal.Status == ProposalStatus.RevisionRequested ||
                  proposal.Status == ProposalStatus.Rejected))
                return Result.Failure<ProposalResponse>(ProposalErrors.CannotUpdate);

            if (proposal.Status == ProposalStatus.Rejected)
            {
                await _unitOfWork.Decisions.DeleteByProposalIdAsync(id, ct);
                var evaluation = await _unitOfWork.Evaluations.GetWithResultsAsync(id, ct);
                if (evaluation is not null)
                {
                    await _unitOfWork.SimilarityResults.DeleteByEvaluationIdAsync(evaluation.Id, ct);
                    await _unitOfWork.Evaluations.DeleteByProposalIdAsync(id, ct);
                }
                proposal.Status = ProposalStatus.Pending;
            }
            if (!string.IsNullOrEmpty(proposal.StoredFileName))
                await _fileService.DeleteFileAsync(proposal.StoredFileName, ct);

            var urlResult = await _fileService.GenerateDownloadUrlAsync(request.StoredFileName, ct);
            if (urlResult.IsFailure)
                return Result.Failure<ProposalResponse>(urlResult.Error);

            proposal.Title = request.Title;
            proposal.Abstract = CleanText(request.Abstract);
            proposal.FileName = request.OriginalFileName;
            proposal.ContentType = request.ContentType;
            proposal.StoredFileName = request.StoredFileName;
            proposal.FileExtension = Path.GetExtension(request.OriginalFileName);
            proposal.ProposalFileUrl = urlResult.Data;
            proposal.Domain = request.Domain;
            _unitOfWork.Proposals.Update(proposal);
            await _unitOfWork.complete(ct);

            var cacheRemoveTasks = Enum.GetNames<ProposalStatus>()
                .Select(s => _cacheService.RemoveAsync(AllProposalsCacheKey(s), ct))
                .Append(_cacheService.RemoveAsync("AllGroups", ct))
                .Append(_cacheService.RemoveAsync(AllProposalsCacheKey(null), ct))
                .Append(_cacheService.RemoveAsync(ProposalCacheKey(id), ct))
                .Append(_cacheService.RemoveAsync(GroupProposalCacheKey(proposal.GroupId), ct))
                .Append(_cacheService.RemoveAsync(ProposalDownloadMetadataCacheKey(id), ct));
            if (proposal.Status == ProposalStatus.Pending)
            {
                cacheRemoveTasks = cacheRemoveTasks
                    .Append(_cacheService.RemoveAsync($"evaluations:proposal:{id}", ct))
                    .Append(_cacheService.RemoveAsync($"decisions:proposal:{id}", ct))
                    .Append(_cacheService.RemoveAsync("decisions:type:rejected", ct));
            }

            await Task.WhenAll(cacheRemoveTasks);

            var savedProposal = await _unitOfWork.Proposals.GetWithDetailsAsync(proposal.Id, ct);

            var evaluationResult = await _evaluationService.RunAutoEvaluationAsync(savedProposal!, ct);

            if (evaluationResult.IsFailure)
            {

            }

            var updated = await _unitOfWork.Proposals.GetWithDetailsAsync(proposal.Id, ct);
            var response = MapToResponse(updated!);

            await _cacheService.SetAsync(ProposalCacheKey(id), response, ct);

            return Result.Success(response);
        }
        public async Task<Result<(string,string)>> DownloadProposalAsync(int id, CancellationToken ct = default)
        {
            string storedFileName;
            int groupId;

            var cachedMetadata = await _cacheService.GetAsync<ProposalDownloadMetadata>(ProposalDownloadMetadataCacheKey(id), ct);

            if (cachedMetadata is not null)
            {
                storedFileName = cachedMetadata.StoredFileName;
                groupId = cachedMetadata.GroupId;
            }
            else
            {
                var proposal = await _unitOfWork.Proposals.GetWithDetailsAsync(id, ct);
                if (proposal is null)
                    return Result.Failure<(string, string)>(ProposalErrors.NotFound);

                storedFileName = proposal.StoredFileName;
                groupId = proposal.GroupId;

                await _cacheService.SetAsync(
                    ProposalDownloadMetadataCacheKey(id),
                    new ProposalDownloadMetadata(
                        StoredFileName: proposal.StoredFileName,
                        ProposalFileUrl: proposal.ProposalFileUrl,
                        ContentType: proposal.ContentType,
                        FileName: proposal.FileName,
                        GroupId: proposal.GroupId),
                    ct);
            }

            var currentUserRole = _currentUserService.GetUserRole();
            var currentUserId = _currentUserService.GetUserId();

            if (currentUserRole == "Supervisor")
                return Result.Failure<(string, string)>(ProposalErrors.CannotDownload);

            if (currentUserRole == "Student")
            {
                var group = await _unitOfWork.Groups.GetByMemberIdAsync(currentUserId, ct);
                if (group is null || group.Id != groupId)
                    return Result.Failure<(string, string)>(ProposalErrors.CannotDownload);
            }

            var urlResult = await _fileService.GenerateDownloadUrlAsync(storedFileName, ct);
            if (urlResult.IsFailure)
                return Result.Failure<(string, string)>(urlResult.Error);

            return Result.Success((urlResult.Data, urlResult.Data + "&download="));
        }
        private static ProposalResponse MapToResponse(Proposal proposal) => new(
            Id: proposal.Id,
            Title: proposal.Title,
            Abstract: proposal.Abstract,
            DownloadUrl: $"/api/proposals/{proposal.Id}/download",
            FileName: proposal.FileName,
            ContentType: proposal.ContentType,
            Status: proposal.Status.ToString(),
            SubmittedAt: proposal.SubmittedAt,
            GroupId: proposal.Group.Id,
            GroupName: proposal.Group.Name,
            LeaderName: proposal.Group.Leader.FullName,
            MembersCount: proposal.Group.Members.Count,
            Domain: proposal.Domain
        );

        private async Task NotifyMembers(EvaluateItEasily.Core.Entities.Group group,
            Proposal proposal, CancellationToken ct)
        {
            var notifications = group.Members.Select(member => new Notification
            {
                UserId = member.StudentId,
                Title = "Proposal Submitted",
                Message = $"Your group proposal '{proposal.Title}' has been submitted successfully",
                Type = NotificationType.ProposalSubmitted,
                CreatedAt = DateTime.UtcNow
            });

            await _unitOfWork.Notifications.AddRangeAsync(notifications, ct);
        }

        private static string CleanText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var cleaned = input.Trim();
            cleaned = Regex.Replace(cleaned, @"\s+", " ");
            cleaned = Regex.Replace(cleaned, @"[^\w\s.,;:!?()\-']", "");

            return cleaned;
        }
        private record ProposalDownloadMetadata(
            string StoredFileName,
            string ProposalFileUrl,
            string ContentType,
            string FileName,
            int GroupId
        );
    }
}
