namespace EvaluateItEasily.Infrastructure.Errors
{
    public static class SupervisorAssignmentErrors
    {
        public static readonly Error NotFound = new(
            "SupervisorAssignment.NotFound",
            "Supervisor assignment was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error AlreadyAssigned = new(
            "SupervisorAssignment.AlreadyAssigned",
            "This proposal already has a supervisor assigned",
            StatusCodes.Status409Conflict);

        public static readonly Error ProposalNotFound = new(
            "SupervisorAssignment.ProposalNotFound",
            "Proposal was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error ProposalNotAccepted = new(
            "SupervisorAssignment.ProposalNotAccepted",
            "Supervisor can only be assigned to accepted proposals",
            StatusCodes.Status400BadRequest);

        public static readonly Error SupervisorNotFound = new(
            "SupervisorAssignment.SupervisorNotFound",
            "Supervisor was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error InvalidSupervisor = new(
            "SupervisorAssignment.InvalidSupervisor",
            "Assigned user must have the Supervisor role",
            StatusCodes.Status400BadRequest);

        public static readonly Error TechnicalAssistantNotFound = new(
            "SupervisorAssignment.TechnicalAssistantNotFound",
            "Technical assistant was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error InvalidTechnicalAssistant = new(
            "SupervisorAssignment.InvalidTechnicalAssistant",
            "Assigned user must have the TechnicalAssistant role",
            StatusCodes.Status400BadRequest);
    }
}
