using EvaluateItEasily.Core.Entities;
using Microsoft.Extensions.Logging;
using System.Globalization;
namespace EvaluateItEasily.Infrastructure.Services
{
    public class HistoricalProjectService : IHistoricalProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<HistoricalProjectService>  _logger;
        private const string AllHistoricalProjectsCacheKey = "historical-projects:all";
        private static string AllProposalsCacheKey(string? status) => string.IsNullOrEmpty(status) ?
            "proposals:all" : $"proposals:status:{status.ToLower()}";
        private static string HistoricalProjectCacheKey(int id) => $"historical-projects:{id}";
        public HistoricalProjectService(IUnitOfWork unitOfWork, ICacheService cacheService,
            ICurrentUserService currentUserService,ILogger<HistoricalProjectService> logger)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _currentUserService = currentUserService;
            _logger = logger;
        }
        public async Task<Result<PaginatedResponse<HistoricalProjectResponse>>> GetAllAsync
            (PaginationRequest request, CancellationToken ct = default)
        {
            var cachedResults = await _cacheService.GetAsync<IEnumerable<HistoricalProjectResponse>>(AllHistoricalProjectsCacheKey, ct);
            IEnumerable<HistoricalProjectResponse> allItems;
            if (cachedResults is not null)
            {
                allItems = cachedResults;
            }
            else
            {
                var dbResults = await _unitOfWork.HistoricalProjects.GetAllWithDetailsAsync(ct);
                allItems = dbResults.Adapt<IEnumerable<HistoricalProjectResponse>>().ToList();
                await _cacheService.SetAsync(AllHistoricalProjectsCacheKey, allItems, ct);
            }
            var totalCount = allItems.Count();
            var items = allItems
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            var response = new PaginatedResponse<HistoricalProjectResponse>(
                Items: items,
                Page: request.Page,
                PageSize: request.PageSize,
                TotalCount: totalCount);

            return Result.Success(response);
        }

        public async Task<Result<int>> ArchiveAcceptedProposalsAsync(ArchiveRequest request, CancellationToken ct = default)
        {
            var accptedProposals = await _unitOfWork.Proposals.GetAcceptedNotArchivedAsync(ct);
            if (!accptedProposals.Any())
                return Result.Failure<int>(HistoricalProjectErrors.NoAcceptedProposals);
            var projects = new List<HistoricalProject>();
            foreach (var item in accptedProposals)
            {
                projects.Add(new HistoricalProject
                {
                    ProposalId = item.Id,
                    Name = item.Title,
                    Abstract = item.Abstract,
                    GroupName = item.Group.Name,
                    AcademicYear = request.AcademicYear,
                    ArchivedAt = DateTime.UtcNow,
                    Domain = item.Domain,
                });
                item.Status = ProposalStatus.Archived;
                _unitOfWork.Proposals.Update(item);
            }
            await _unitOfWork.HistoricalProjects.BulkInsertAsync(projects, ct);
            await _unitOfWork.complete(ct);

            foreach (var status in Enum.GetNames<ProposalStatus>())
                await _cacheService.RemoveAsync(AllProposalsCacheKey(status), ct);

            await _cacheService.RemoveAsync(AllProposalsCacheKey(null), ct);
            await _cacheService.RemoveAsync(AllHistoricalProjectsCacheKey, ct);

            return Result.Success(projects.Count);
        }

        public async Task<Result<HistoricalProjectResponse>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var cached = await _cacheService.GetAsync<HistoricalProjectResponse>(HistoricalProjectCacheKey(id), ct);

            if (cached is not null)
                return Result.Success(cached);

            var project = await _unitOfWork.HistoricalProjects.GetByProjectIdAsync(id, ct);
            if (project is null)
                return Result.Failure<HistoricalProjectResponse>(HistoricalProjectErrors.NotFound);

            var response = project.Adapt<HistoricalProjectResponse>();

            await _cacheService.SetAsync(HistoricalProjectCacheKey(id), response, ct);

            return Result.Success(response);
        }

        public async Task<Result<int>> ImportCsvAsync(IFormFile file, CancellationToken ct = default)
        {
            if (file is null || file.Length == 0)
                return Result.Failure<int>(HistoricalProjectErrors.InvalidCsvFile);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".csv")
                return Result.Failure<int>(HistoricalProjectErrors.InvalidCsvFile);
            try
            {
                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                await csv.ReadAsync();
                csv.ReadHeader();

                var headers = csv.HeaderRecord ?? [];
                if (!headers.Contains("Name") ||
                    !headers.Contains("Abstract") ||
                    !headers.Contains("Date"))
                    return Result.Failure<int>(HistoricalProjectErrors.MissingCsvColumns);

                var projects = new List<HistoricalProject>();
                while (await csv.ReadAsync())
                {
                    var name = csv.GetField("Name")?.Trim();
                    var abstract_ = csv.GetField("Abstract")?.Trim();
                    var date = csv.GetField("Date")?.Trim();

                    if (string.IsNullOrWhiteSpace(name) ||
                        string.IsNullOrWhiteSpace(abstract_))
                        continue;

                    projects.Add(new HistoricalProject
                    {
                        Name = name,
                        Abstract = abstract_,
                        GroupName = string.Empty,     
                        AcademicYear = date ?? string.Empty,
                        ArchivedAt = DateTime.UtcNow,
                        ProposalId = null             
                    });
                }

                if (projects.Count == 0)
                    return Result.Failure<int>(HistoricalProjectErrors.EmptyCsvFile);

                await _unitOfWork.HistoricalProjects.BulkInsertAsync(projects, ct);
                await _unitOfWork.complete(ct);

                await _cacheService.RemoveAsync(AllHistoricalProjectsCacheKey, ct);

                return Result.Success(projects.Count);
            }
            catch
            {
                return Result.Failure<int>(HistoricalProjectErrors.ImportFailed);
            }
        }

    }
}