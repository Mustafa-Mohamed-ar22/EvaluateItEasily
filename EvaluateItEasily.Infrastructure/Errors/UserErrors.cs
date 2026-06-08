namespace EvaluateItEasily.Infrastructure.Errors
{
    public static class UserErrors
    {
        public static readonly Error NotFound = new(
            "User.NotFound",
            "User was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error EmailAlreadyExists = new(
            "User.EmailAlreadyExists",
            "This email is already registered",
            StatusCodes.Status409Conflict);

        public static readonly Error InvalidRole = new(
            "User.InvalidRole",
            "Role must be Supervisor, Committee, or Admin",
            StatusCodes.Status400BadRequest);

        public static readonly Error CannotDeactivateSelf = new(
            "User.CannotDeactivateSelf",
            "You cannot deactivate your own account",
            StatusCodes.Status400BadRequest);
        public static readonly Error InvalidCsvFile = new(
                "User.InvalidCsvFile",
                "File must be a valid .csv file",
                StatusCodes.Status422UnprocessableEntity);

        public static readonly Error EmptyCsvFile = new(
            "User.EmptyCsvFile",
            "CSV file contains no valid records",
            StatusCodes.Status422UnprocessableEntity);

        public static readonly Error MissingCsvColumns = new(
            "User.MissingCsvColumns",
            "CSV must contain Name, SSN, Code columns",
            StatusCodes.Status422UnprocessableEntity);
    }
}
