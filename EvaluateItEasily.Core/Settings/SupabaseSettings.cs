namespace EvaluateItEasily.Core.Settings
{
    public class SupabaseSettings
    {
        public static readonly string SectionName = "SupabaseSettings";

        public string Url { get; init; } = string.Empty;  
        public string ServiceRoleKey { get; init; } = string.Empty;  
        public string BucketName { get; init; } = string.Empty;  
        public int DownloadExpirySeconds { get; init; } = 3600;  
    }
}
