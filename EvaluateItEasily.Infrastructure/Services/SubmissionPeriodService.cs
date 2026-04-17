using EvaluateItEasily.Core.DTO_s.SubmissionPeriod;
using FluentValidation;

namespace EvaluateItEasily.Infrastructure.Services
{
    public class SubmissionPeriodService(IUnitOfWork unitOfWork, ICacheService cacheService, IValidator<SetSubmissionPeriodRequest> validator) : ISubmissionPeriodService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IValidator<SetSubmissionPeriodRequest> _validator = validator;

        private const string AllPeriodsCacheKey = "submission-periods:all";
        private const string CurrentPeriodCacheKey = "submission-periods:current";

        public async Task<Result<IEnumerable<SubmissionPeriodResponse>>> GetAllAsync(CancellationToken ct = default)
        {
            var cached = await _cacheService.GetAsync<IEnumerable<SubmissionPeriodResponse>>(AllPeriodsCacheKey, ct);

            if (cached is not null)
                return Result.Success(cached);

            var periods = await _unitOfWork.SubmissionPeriods.GetAllAsync(ct);
            var response = periods.Select(MapToResponse).ToList();

            await _cacheService.SetAsync(AllPeriodsCacheKey, response, ct);

            return Result.Success<IEnumerable<SubmissionPeriodResponse>>(response);
        }

        public async Task<Result<SubmissionPeriodResponse>> GetCurrentAsync(CancellationToken ct = default)
        {
            var period = await _unitOfWork.SubmissionPeriods.GetCurrentOpenAsync(ct);

            if (period is null)
                return Result.Failure<SubmissionPeriodResponse>(
                    SubmissionPeriodErrors.NoActivePeriod);

            return Result.Success(MapToResponse(period));
        }

        public async Task<Result<SubmissionPeriodResponse>> CreateAsync(SetSubmissionPeriodRequest request,CancellationToken ct = default)
        {
            // Validate
            var validation = await _validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
            {
                var errors = string.Join(", ", validation.Errors.Select(e => e.ErrorMessage));
                return Result.Failure<SubmissionPeriodResponse>(new Error("SubmissionPeriod.Validation", errors, StatusCodes.Status400BadRequest));
            }

            // Check overlap with existing active periods
            var hasOverlap = await _unitOfWork.SubmissionPeriods.HasOverlapAsync(request.StartDate, request.EndDate, ct: ct);

            if (hasOverlap)
                return Result.Failure<SubmissionPeriodResponse>(SubmissionPeriodErrors.OverlappingPeriod);

            var period = new SubmissionPeriod
            {
                Title = request.Title,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = true
            };

            await _unitOfWork.SubmissionPeriods.AddAsync(period, ct);
            await _unitOfWork.complete(ct);

            // Invalidate cache
            await _cacheService.RemoveAsync(AllPeriodsCacheKey, ct);
            await _cacheService.RemoveAsync(CurrentPeriodCacheKey, ct);

            var created = await _unitOfWork.SubmissionPeriods.GetCurrentOpenAsync(ct);
            return Result.Success(MapToResponse(period));
        }

        public async Task<Result<SubmissionPeriodResponse>> UpdateAsync(int id,SetSubmissionPeriodRequest request,CancellationToken ct = default)
        {
            // Validate
            var validation = await _validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
            {
                var errors = string.Join(", ", validation.Errors.Select(e => e.ErrorMessage));
                return Result.Failure<SubmissionPeriodResponse>(
                    new Error("SubmissionPeriod.Validation", errors, StatusCodes.Status400BadRequest));
            }

            var period = await _unitOfWork.SubmissionPeriods.GetByIdAsync(id, ct);
            if (period is null)
                return Result.Failure<SubmissionPeriodResponse>(SubmissionPeriodErrors.NotFound);

            // Check overlap excluding current period
            var hasOverlap = await _unitOfWork.SubmissionPeriods.HasOverlapAsync(request.StartDate, request.EndDate, excludeId: id, ct: ct);

            if (hasOverlap)
                return Result.Failure<SubmissionPeriodResponse>(
                    SubmissionPeriodErrors.OverlappingPeriod);

            period.Title = request.Title;
            period.StartDate = request.StartDate;
            period.EndDate = request.EndDate;

            _unitOfWork.SubmissionPeriods.Update(period);
            await _unitOfWork.complete(ct);

            // Invalidate cache
            await _cacheService.RemoveAsync(AllPeriodsCacheKey, ct);
            await _cacheService.RemoveAsync(CurrentPeriodCacheKey, ct);

            return Result.Success(MapToResponse(period));
        }

        public async Task<Result> ToggleActiveAsync(int id, CancellationToken ct = default)
        {
            var period = await _unitOfWork.SubmissionPeriods.GetByIdAsync(id, ct);
            if (period is null)
                return Result.Failure(SubmissionPeriodErrors.NotFound);

            period.IsActive = !period.IsActive;

            _unitOfWork.SubmissionPeriods.Update(period);
            await _unitOfWork.complete(ct);

            // Invalidate cache
            await _cacheService.RemoveAsync(AllPeriodsCacheKey, ct);
            await _cacheService.RemoveAsync(CurrentPeriodCacheKey, ct);

            return Result.Success();
        }

        public async Task<Result> ValidateIsOpenAsync(CancellationToken ct = default)
        {
            var period = await _unitOfWork.SubmissionPeriods.GetCurrentOpenAsync(ct);

            if (period is null)
                return Result.Failure(SubmissionPeriodErrors.SubmissionClosed);

            return Result.Success();
        }

        // ── Private helper ────────────────────────────────────────────────
        private static SubmissionPeriodResponse MapToResponse(SubmissionPeriod period) => new(
            Id: period.Id,
            Title: period.Title,
            StartDate: period.StartDate,
            EndDate: period.EndDate,
            IsActive: period.IsActive,
            IsOpen: period.IsOpen,        // ← computed live every time
            CreatedOn: period.CreatedOn,
            CreatedByName: period.CreatedBy?.FullName ?? string.Empty
        );
    }
}
