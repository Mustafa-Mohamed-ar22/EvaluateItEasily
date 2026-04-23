using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace EvaluateItEasily.API.Extensions
{
    public static class KestrelExtensions
    {
        private const long MaxFileSize = 10_485_760;      // 10 MB
        private const long MaxRequestSize = 12_534_336;   // 11 MB

        public static WebApplicationBuilder AddKestrelConfiguration(this WebApplicationBuilder builder)
        {
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = MaxRequestSize;
                options.Limits.MinRequestBodyDataRate = new MinDataRate(
                    bytesPerSecond: 1024,
                    gracePeriod: TimeSpan.FromSeconds(18)
                );
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = MaxRequestSize;
                options.BufferBodyLengthLimit = MaxRequestSize;
                options.MemoryBufferThreshold = 1_048_576;
            });

            return builder;
        }
    }
}