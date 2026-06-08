namespace EvaluateItEasily.Core.Settings
{
    public class SimilarityThresholdSettings
    {
        public static readonly string SectionName = "SimilarityThresholdSettings";
        public float AutoRejectThreshold { get; init; } = 0.85f;  
    }
}
