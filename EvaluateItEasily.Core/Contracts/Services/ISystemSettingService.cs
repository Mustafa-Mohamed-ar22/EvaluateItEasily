using EvaluateItEasily.Core.Results;
using EvaluateItEasily.Core.Settings;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface ISystemSettingService
    {
        Task<Result<ThresholdResponse>> GetThresholdAsync(CancellationToken ct = default);
        Task<Result<ThresholdResponse>> SetThresholdAsync(SetThresholdRequest request, CancellationToken ct = default);
        Task<float> GetThresholdValueAsync(CancellationToken ct = default); 
    }
}
