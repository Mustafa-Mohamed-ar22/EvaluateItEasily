using EvaluateItEasily.Core.DTO_s.SupervisorAssignments;
using FluentValidation;

namespace EvaluateItEasily.Infrastructure.Validators
{
    public class CreateSupervisorAssignmentRequestValidator :
        AbstractValidator<CreateSupervisorAssignmentRequest>
    {
        public CreateSupervisorAssignmentRequestValidator()
        {
            RuleFor(x => x.ProposalId)
                .GreaterThan(0).WithMessage("Proposal id is required");

            RuleFor(x => x.SupervisorId)
                .NotEmpty().WithMessage("Supervisor id is required");

            RuleFor(x => x.TechnicalAssistantId)
            .NotEmpty().WithMessage("Technical assistant id is required");

            RuleFor(x => x.WorkloadNote)
                .MaximumLength(500).WithMessage("Workload note cannot exceed 500 characters")
                .When(x => x.WorkloadNote is not null);
        }
    }
}
