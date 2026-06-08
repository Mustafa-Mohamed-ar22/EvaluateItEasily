using EvaluateItEasily.Core.DTO_s.Account;
using EvaluateItEasily.Core.DTO_s.Users;
using EvaluateItEasily.Core.Results;
using Microsoft.AspNetCore.Http;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface IUserService
    {
        Task<Result> ChangePasswordAsync(ChangePasswordRequest request);
        Task<Result<IEnumerable<UserResponse>>> GetAllAsync(string? role, CancellationToken ct = default);
        Task<Result<UserResponse>> GetByIdAsync(string id, CancellationToken ct = default);
        Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
        Task<Result<UserResponse>> UpdateAsync(string id, UpdateUserRequest request, CancellationToken ct = default);
        Task<Result> ToggleActiveAsync(string id, CancellationToken ct = default);
        Task<Result<ImportStudentsResponse>> ImportStudentsAsync(IFormFile file,CancellationToken ct = default);
    }
}