using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s.Proposals;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
namespace EvaluateItEasily.Infrastructure.Services
{
    public class ProposalService : IProposalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileService _fileService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<ProposalService> Logger;
        private readonly ISubmissionPeriodService _submissionPeriodService;
        private static string ProposalCacheKey(int id) => $"proposals:{id}";
        private static string GroupProposalCacheKey(int groupId) => $"proposals:group:{groupId}";
        private static string AllProposalsCacheKey(string? status) =>    string.IsNullOrEmpty(status)        ? "proposals:all"        : $"proposals:status:{status.ToLower()}";

        public ProposalService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IFileService fileService, ICacheService cacheService, ILogger<ProposalService> logger, ISubmissionPeriodService submissionPeriodService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _fileService = fileService;
            _cacheService = cacheService;
            Logger = logger;
            _submissionPeriodService = submissionPeriodService;
        }

        public async Task<Result<IEnumerable<ProposalResponse>>> GetAllAsync(string? status = null, CancellationToken ct = default)
        {
            if (!string.IsNullOrEmpty(status) &&!Enum.TryParse<ProposalStatus>(status, ignoreCase: true, out _))
                return Result.Failure<IEnumerable<ProposalResponse>>(ProposalErrors.InvalidStatus);

            var cacheKey = AllProposalsCacheKey(status);

            // Check cache first
            var cached = await _cacheService.GetAsync<IEnumerable<ProposalResponse>>(cacheKey, ct);
            if (cached is not null)
                return Result.Success(cached);

            // Cache miss → hit DB
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
                Logger.LogError("I Entered to the Cached Results for Id ");
                 
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
                Logger.LogError("I Entered to the Cached Results for user ");

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
                var fileResult = await _fileService.SaveFileAsync(request.ProposalFile, ct);
                if (fileResult.IsFailure)
                    return Result.Failure<ProposalResponse>(fileResult.Error);

                var proposal = new Proposal
                {
                    FileName = request.ProposalFile.FileName,
                    ContentType = request.ProposalFile.ContentType,
                    StoredFileName = fileResult.Data.Item2,
                    FileExtension = Path.GetExtension(request.ProposalFile.FileName),
                    Abstract = CleanText(request.Abstract),
                    Title = request.Title,
                    SubmittedAt = DateTime.UtcNow,
                    Status = ProposalStatus.Pending,
                    GroupId = group.Id,
                    ProposalFileUrl = fileResult.Data.Item1,
                };

                await _unitOfWork.Proposals.AddAsync(proposal, ct);
                await NotifyMembers(group, proposal, ct);
                await _unitOfWork.complete(ct);

                foreach (var status in Enum.GetNames<ProposalStatus>())
                    await _cacheService.RemoveAsync(AllProposalsCacheKey(status), ct);

                await _cacheService.RemoveAsync(AllProposalsCacheKey(null), ct);
                await _cacheService.RemoveAsync(GroupProposalCacheKey(group.Id), ct);

                var created = await _unitOfWork.Proposals.GetWithDetailsAsync(proposal.Id, ct);
                var response = MapToResponse(created!);

                await _cacheService.SetAsync(ProposalCacheKey(proposal.Id), response, ct);

                return Result.Success(response);
            }
            catch
            {
                return Result.Failure<ProposalResponse>(ProposalErrors.CannotUpload);
            }
        }

        public async Task<Result<ProposalResponse>> UpdateAsync(int id,UpdateProposalRequest request,CancellationToken ct = default)
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
                  proposal.Status == ProposalStatus.RevisionRequested))
                return Result.Failure<ProposalResponse>(ProposalErrors.CannotUpdate);

            var fileResult = await _fileService.SaveFileAsync(request.ProposalFile, ct);
            if (fileResult.IsFailure)
                return Result.Failure<ProposalResponse>(fileResult.Error);

            _fileService.DeleteFile(proposal.ProposalFileUrl);

            proposal.Title = request.Title;
            proposal.Abstract = CleanText(request.Abstract);
            proposal.FileName = request.ProposalFile.FileName;
            proposal.ContentType = request.ProposalFile.ContentType;
            proposal.StoredFileName = fileResult.Data.Item2;
            proposal.FileExtension = Path.GetExtension(request.ProposalFile.FileName);
            proposal.ProposalFileUrl = fileResult.Data.Item1;

            _unitOfWork.Proposals.Update(proposal);
            await _unitOfWork.complete(ct);

            foreach (var status in Enum.GetNames<ProposalStatus>())
                await _cacheService.RemoveAsync(AllProposalsCacheKey(status), ct);

            await _cacheService.RemoveAsync(AllProposalsCacheKey(null), ct);
            await _cacheService.RemoveAsync(ProposalCacheKey(id), ct);
            await _cacheService.RemoveAsync(GroupProposalCacheKey(proposal.GroupId), ct);

            var updated = await _unitOfWork.Proposals.GetWithDetailsAsync(proposal.Id, ct);
            var response = MapToResponse(updated!);

            await _cacheService.SetAsync(ProposalCacheKey(id), response, ct);

            return Result.Success(response);
        }

        public async Task<Result<FileDownloadResponse>> DownloadProposalAsync(int id,CancellationToken ct = default)
        {
            var cached = await _cacheService.GetAsync<ProposalResponse>(ProposalCacheKey(id), ct);

            string proposalFileUrl;
            string contentType;
            string fileName;
            int groupId;

            if (cached is not null)
            {
                Logger.LogError("I Entered to the Cached Results for download ");

                var proposal = await _unitOfWork.Proposals.GetByIdAsync(id, ct);
                if (proposal is null)
                    return Result.Failure<FileDownloadResponse>(ProposalErrors.NotFound);

                proposalFileUrl = proposal.ProposalFileUrl;
                contentType = cached.ContentType;
                fileName = cached.FileName;
                groupId = cached.GroupId;
            }
            else
            {
                var proposal = await _unitOfWork.Proposals.GetWithDetailsAsync(id, ct);
                if (proposal is null)
                    return Result.Failure<FileDownloadResponse>(ProposalErrors.NotFound);

                proposalFileUrl = proposal.ProposalFileUrl;
                contentType = proposal.ContentType;
                fileName = proposal.FileName;
                groupId = proposal.GroupId;
            }

            var currentUserId = _currentUserService.GetUserId();
            var currentUserRole = _currentUserService.GetUserRole();

            if (currentUserRole == "Student")
            {
                var group = await _unitOfWork.Groups.GetByMemberIdAsync(currentUserId, ct);
                if (group is null || group.Id != groupId)
                    return Result.Failure<FileDownloadResponse>(ProposalErrors.CannotDownload);
            }

            if (currentUserRole == "Supervisor")
                return Result.Failure<FileDownloadResponse>(ProposalErrors.CannotDownload);

            var fileResult = await _fileService.GetFileAsync(proposalFileUrl, ct);
            if (fileResult.IsFailure)
                return Result.Failure<FileDownloadResponse>(fileResult.Error);

            return Result.Success(new FileDownloadResponse(FileBytes: fileResult.Data,ContentType: contentType,FileName: fileName));
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
            MembersCount: proposal.Group.Members.Count
        );

        private async Task NotifyMembers(EvaluateItEasily.Core.Entities.Group group, Proposal proposal, CancellationToken ct)
        {
            foreach (var member in group.Members)
            {
                await _unitOfWork.Notifications.AddAsync(new Notification
                {
                    UserId = member.StudentId,
                    Title = "Proposal Submitted",
                    Message = $"Your group proposal '{proposal.Title}' has been submitted successfully",
                    Type = NotificationType.ProposalSubmitted,
                    CreatedAt = DateTime.UtcNow
                }, ct);
            }
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
    }
}
