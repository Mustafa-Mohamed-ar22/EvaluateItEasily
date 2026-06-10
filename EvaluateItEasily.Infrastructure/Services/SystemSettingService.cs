using EvaluateItEasily.Core.Settings;
using Microsoft.Extensions.Options;

namespace EvaluateItEasily.Infrastructure.Services
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly SimilarityThresholdSettings _defaultSettings;

        private const string ThresholdCacheKey = "settings:similarity-threshold";

        public SystemSettingService(
            IUnitOfWork unitOfWork,
            ICacheService cacheService,
            IOptions<SimilarityThresholdSettings> defaultSettings)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _defaultSettings = defaultSettings.Value;
        }

        public async Task<Result<ThresholdResponse>> GetThresholdAsync(
            CancellationToken ct = default)
        {
            var value = await GetThresholdValueAsync(ct);
            return Result.Success(new ThresholdResponse(value));
        }

        public async Task<Result<ThresholdResponse>> SetThresholdAsync(
            SetThresholdRequest request,
            CancellationToken ct = default)
        {
            if (request.Threshold <= 0 || request.Threshold > 1)
                return Result.Failure<ThresholdResponse>(new Error(
                    "Settings.InvalidThreshold",
                    "Threshold must be between 0 and 1",
                    StatusCodes.Status422UnprocessableEntity));

            var setting = await _unitOfWork.SystemSettings
                .GetByKeyAsync(SystemSettingKeys.SimilarityThreshold, ct);

            if (setting is null)
            {
                await _unitOfWork.SystemSettings.AddAsync(new SystemSetting
                {
                    Key = SystemSettingKeys.SimilarityThreshold,
                    Value = request.Threshold.ToString("F2")
                }, ct);
            }
            else
            {
                setting.Value = request.Threshold.ToString("F2");
                _unitOfWork.SystemSettings.Update(setting);
            }

            await _unitOfWork.complete(ct);

            await _cacheService.RemoveAsync(ThresholdCacheKey, ct);

            return Result.Success(new ThresholdResponse(request.Threshold));
        }

        public async Task<float> GetThresholdValueAsync(CancellationToken ct = default)
        {
            var cached = await _cacheService.GetAsync<ThresholdResponse>(ThresholdCacheKey, ct);
            if (cached is not null)
                return cached.Threshold;

            var setting = await _unitOfWork.SystemSettings
                .GetByKeyAsync(SystemSettingKeys.SimilarityThreshold, ct);

            var value = setting is not null
                ? float.Parse(setting.Value)
                : _defaultSettings.AutoRejectThreshold;

            await _cacheService.SetAsync(ThresholdCacheKey, new ThresholdResponse(value), ct);

            return value;
        }
    }
}