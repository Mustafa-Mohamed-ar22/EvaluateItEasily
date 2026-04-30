namespace EvaluateItEasily.Infrastructure.Errors
{
    public static class FileErrors
    {
        public static readonly Error InvalidFile = new(
            "File.InvalidFile",
            "File is invalid or empty",
            StatusCodes.Status400BadRequest);

        public static readonly Error InvalidExtension = new(
            "File.InvalidExtension",
            "Only PDF files are allowed",
            StatusCodes.Status400BadRequest);

        public static readonly Error FileTooLarge = new(
            "File.FileTooLarge",
            "File size cannot exceed 10MB",
            StatusCodes.Status400BadRequest);

        public static readonly Error SaveFailed = new(
            "File.SaveFailed",
            "Failed to upload the file",
            StatusCodes.Status500InternalServerError);

        public static readonly Error FileNotFound = new(
            "File.FileNotFound",
            "File was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error DeleteFailed = new(
            "File.DeleteFailed",
            "Failed to delete the file",
            StatusCodes.Status500InternalServerError);
    }
}
