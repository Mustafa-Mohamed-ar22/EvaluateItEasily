namespace EvaluateItEasily.Core.Settings
{
    public class AISettings
    {
        public static readonly string SectionName = "AISettings";
        public string BaseUrl { get; init; } = string.Empty;      
        public int TopK { get; init; } = 5;
        public int TimeoutSeconds { get; init; } = 30;
    }
}
