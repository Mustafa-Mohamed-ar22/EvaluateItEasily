using EvaluateItEasily.Core.DTO_s.Proposals;
using System.Text.RegularExpressions;
namespace EvaluateItEasily.Infrastructure.Services
{
    public class ProposalService : IProposalService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentUserService _currentUserService;
        private readonly IFileService _fileService;

        public ProposalService(IUnitOfWork unitOfWork, ICurrentUserService CuurrentUserService, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = CuurrentUserService;
            _fileService = fileService;
        }

        public async Task<Result<ProposalResponse>> CreateAsync(CreateProposalRequest request, CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();
            var group = await _unitOfWork.Groups.GetByMemberIdAsync(currentUserId,ct);
            if(group is null)
                return Result.Failure<ProposalResponse>(ProposalErrors.NoGroup);
            if(group.LeaderId != currentUserId)
                return Result.Failure<ProposalResponse>(ProposalErrors.NotLeader);

            var existing = await _unitOfWork.Proposals.GetByGroupIdAsync(group.Id, ct);
            if (existing is not null)
                return Result.Failure<ProposalResponse>(ProposalErrors.AlreadySubmitted);
            try
            {
                var fileResult = await _fileService.SaveFileAsync(request.ProposalFile, ct);
                
                
                if (fileResult.IsFailure)
                    return Result.Failure<ProposalResponse>(fileResult.Error);
                Console.ForegroundColor = ConsoleColor.Red;

                await Console.Out.WriteLineAsync(fileResult.Data.Item1);
                await Console.Out.WriteLineAsync(fileResult.Data.Item2);


                Console.ForegroundColor = ConsoleColor.White;

                var Proposal = new Proposal
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
                await _unitOfWork.Proposals.AddAsync(Proposal, ct);
                await NotifyMembers(group, Proposal, ct);
                await _unitOfWork.complete(ct);

                var created = await _unitOfWork.Proposals.GetWithDetailsAsync(Proposal.Id, ct);

                return Result.Success(MapToResponse(created!));
            }
            catch (Exception ex)
            {
                return Result.Failure<ProposalResponse>(ProposalErrors.CannotUpload);
            }
        }

        private string CleanText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string cleaned = input.Trim();

            cleaned = Regex.Replace(cleaned, @"\s+", " ");

            cleaned = Regex.Replace(cleaned, @"[^\w\s.,;:!?()\-']", "");

            return cleaned;
        }

        public async Task<Result<IEnumerable<ProposalResponse>>> GetAllAsync(CancellationToken ct = default)
        {
            var proposals = await _unitOfWork.Proposals.GetAllWithDetailsAsync(ct);
            return Result.Success(proposals.Select(MapToResponse));
        }

        public async Task<Result<ProposalResponse>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var proposal = await _unitOfWork.Proposals.GetWithDetailsAsync(id, ct);

            return proposal is null
                ? Result.Failure<ProposalResponse>(ProposalErrors.NotFound)
                : Result.Success(MapToResponse(proposal));
        }

        public async Task<Result<ProposalResponse>> GetMyProposalAsync(CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();

            var group = await _unitOfWork.Groups.GetByMemberIdAsync(currentUserId, ct);
            if (group is null)
                return Result.Failure<ProposalResponse>(ProposalErrors.NoGroup);

            var proposal = await _unitOfWork.Proposals.GetByGroupIdAsync(group.Id, ct);
            if (proposal is null)
                return Result.Failure<ProposalResponse>(ProposalErrors.NoProposal);

            return Result.Success(MapToResponse(proposal));
        }

        public async Task<Result<ProposalResponse>> UpdateAsync(int id, UpdateProposalRequest request, CancellationToken ct = default)
        {
            var currentUserId = _currentUserService.GetUserId();

            var proposal = await _unitOfWork.Proposals.GetWithDetailsAsync(id, ct);
            if (proposal is null)
                return Result.Failure<ProposalResponse>(ProposalErrors.NotFound);

            if (proposal.Group.LeaderId != currentUserId)
                return Result.Failure<ProposalResponse>(ProposalErrors.NotLeader);


            if (!(proposal.Status.ToString() == UpdatableStatuses.Pending.ToString() || 
                proposal.Status.ToString() == UpdatableStatuses.RevisionRequested.ToString()))
                
                return Result.Failure<ProposalResponse>(ProposalErrors.CannotUpdate);

            proposal.Title = request.Title;
            proposal.Abstract = request.Abstract;
            _fileService.DeleteFile(proposal.ProposalFileUrl);
            

            _unitOfWork.Proposals.Update(proposal);
            await _unitOfWork.complete(ct);
            var fileResult = await _fileService.SaveFileAsync(request.ProposalFile, ct);
            if (fileResult.IsFailure)
                return Result.Failure<ProposalResponse>(fileResult.Error);
            
            proposal.FileName = request.ProposalFile.FileName;
            proposal.ContentType = request.ProposalFile.ContentType;
            proposal.StoredFileName = fileResult.Data.Item2;
            proposal.FileExtension = Path.GetExtension(request.ProposalFile.FileName);
            proposal.Abstract = CleanText(request.Abstract);
            proposal.Title = request.Title;
            proposal.ProposalFileUrl = fileResult.Data.Item1;
            _unitOfWork.Proposals.Update(proposal);
            await _unitOfWork.complete(ct);

            var created = await _unitOfWork.Proposals.GetWithDetailsAsync(proposal.Id, ct);

            return Result.Success(MapToResponse(created!));
        }
        public async Task<Result<FileDownloadResponse>> DownloadProposalAsync(int id,CancellationToken ct = default)
        {
            var proposal = await _unitOfWork.Proposals.GetWithDetailsAsync(id, ct);
            if (proposal is null)
                return Result.Failure<FileDownloadResponse>(ProposalErrors.NotFound);

            var currentUserId = _currentUserService.GetUserId();
            var currentUserRole = _currentUserService.GetUserRole();

            if (currentUserRole == "Student")
            {
                var group = await _unitOfWork.Groups.GetByMemberIdAsync(currentUserId, ct);

                if (group is null || group.Id != proposal.GroupId)
                    return Result.Failure<FileDownloadResponse>(ProposalErrors.CannotDownload);
            }
            var fileResult = await _fileService.GetFileAsync(proposal.ProposalFileUrl, ct);
            if (fileResult.IsFailure)
                return Result.Failure<FileDownloadResponse>(fileResult.Error);

            return Result.Success(new FileDownloadResponse(
                FileBytes: fileResult.Data,
                ContentType: proposal.ContentType,
                FileName: proposal.FileName
            ));
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
        private async Task NotifyMembers(EvaluateItEasily.Core.Entities.Group group, Proposal Proposal, CancellationToken ct)
        {
            foreach (var member in group.Members)
            {
                await _unitOfWork.Notifications.AddAsync(new Notification
                {
                    UserId = member.StudentId,
                    Title = "Proposal Submitted",
                    Message = $"Your group proposal '{Proposal.Title}' has been submitted successfully",
                    Type = NotificationType.ProposalSubmitted,
                    CreatedAt = DateTime.UtcNow
                }, ct);
            }
        }
    }
}
