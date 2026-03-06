using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluateItEasily.Core.Results
{
    public record Error(string code, string ErrorDescription, int? StatusCode)
    {
        public static readonly Error None = new(string.Empty, string.Empty, null);
    }
}
