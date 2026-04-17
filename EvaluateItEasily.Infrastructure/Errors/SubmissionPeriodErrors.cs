namespace EvaluateItEasily.Infrastructure.Errors
{
    public static class SubmissionPeriodErrors
    {
        public static readonly Error NotFound = new(
            "SubmissionPeriod.NotFound",
            "Submission period was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error NoActivePeriod = new(
            "SubmissionPeriod.NoActivePeriod",
            "No active submission period is currently set",
            StatusCodes.Status400BadRequest);

        public static readonly Error SubmissionClosed = new(
            "SubmissionPeriod.SubmissionClosed",
            "Proposal submission is currently closed",
            StatusCodes.Status403Forbidden);

        public static readonly Error InvalidDateRange = new(
            "SubmissionPeriod.InvalidDateRange",
            "End date must be after start date",
            StatusCodes.Status400BadRequest);

        public static readonly Error OverlappingPeriod = new(
            "SubmissionPeriod.OverlappingPeriod",
            "An active submission period already exists that overlaps with this range",
            StatusCodes.Status409Conflict);
    }
}
