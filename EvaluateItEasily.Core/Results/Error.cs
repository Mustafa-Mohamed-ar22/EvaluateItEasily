

namespace EvaluateItEasily.Core.Results
{
    public record Error(string code, string ErrorDescription, int? StatusCode)
    {
        public static readonly Error None = new(string.Empty, string.Empty, null);
    }
}
