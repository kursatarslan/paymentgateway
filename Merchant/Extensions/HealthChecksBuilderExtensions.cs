using System.Collections.Generic;
using Merchant.HealthChecks.GCInfo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Merchant.Extensions
{
    public static class HealthChecksBuilderExtensions
    {
        public static IHealthChecksBuilder AddGCInfoCheck(
            this IHealthChecksBuilder builder,
            string name,
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            long? thresholdInBytes = null)
        {
            builder.AddCheck<GCInfoHealthCheck>(name, failureStatus ?? HealthStatus.Degraded, tags);

            if (thresholdInBytes.HasValue)
                builder.Services.Configure<GCInfoOptions>(name, options => options.Threshold = thresholdInBytes.Value);

            return builder;
        }
    }
}