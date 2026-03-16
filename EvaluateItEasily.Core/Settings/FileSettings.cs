
namespace EvaluateItEasily.Infrastructure.Settings
{
    public class FileSettings
    {
        public static string StoragePath { get; } = "Uploads/Proposals";
        public static long MaxFileSizeInBytes { get; } = 10*1024*1024;
        public static string AllowedFileExtensionSignatures = "25-50-44-46";
        public static string CSVSignature = "";
    }
}
