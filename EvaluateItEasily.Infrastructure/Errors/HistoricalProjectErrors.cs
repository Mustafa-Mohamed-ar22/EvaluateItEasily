using Microsoft.AspNetCore.Http;

namespace EvaluateItEasily.Infrastructure.Errors
{
    public static class HistoricalProjectErrors
    {
        public static readonly Error NotFound = new(
            "HistoricalProject.NotFound",
            "Historical project was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error InvalidCsvFile = new(
            "HistoricalProject.InvalidCsvFile",
            "File must be a valid .csv file",
            StatusCodes.Status409Conflict);

        public static readonly Error EmptyCsvFile = new(
            "HistoricalProject.EmptyCsvFile",
            "CSV file contains no valid records",
            StatusCodes.Status409Conflict);

        public static readonly Error MissingCsvColumns = new(
            "HistoricalProject.MissingCsvColumns",
            "CSV must contain Name, Abstract, Date columns",
            StatusCodes.Status409Conflict);

        public static readonly Error NoAcceptedProposals = new(
            "HistoricalProject.NoAcceptedProposals",
            "No accepted proposals found to archive for this year",
            StatusCodes.Status409Conflict);

        public static readonly Error ImportFailed = new(
            "HistoricalProject.ImportFailed",
            "Failed to import CSV file",
            StatusCodes.Status500InternalServerError);
    }
}
