using EvaluateItEasily.Core.DTO_s.SupervisorAssignments;
using EvaluateItEasily.Core.DTO_s;
using EvaluateItEasily.Core.Results;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface ISupervisorAssignmentService
    {
        Task<Result<IEnumerable<SupervisorAssignmentResponse>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<SupervisorAssignmentResponse>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Result<IEnumerable<SupervisorAssignmentResponse>>> GetMyAssignmentsAsync(CancellationToken ct = default);
        Task<Result<SupervisorAssignmentResponse>> CreateAsync(CreateSupervisorAssignmentRequest request, CancellationToken ct = default);
    }
}

