using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace Merchant.Extensions
{
     public static class HealthChecksServices
    {
        public static IHealthChecksBuilder AddDefaultHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(30);
                options.Predicate = (check) => check.Tags.Contains("self");
            });

            var builder = services.AddHealthChecks()
                .AddCheck("Merchant Api",
                    () => HealthCheckResult.Healthy("Merchant Service" + " BFF is OK!"), tags: new[] { "BFF" })
                .AddGCInfoCheck("GCInfo");

            if (!string.IsNullOrEmpty(configuration["Authorize:Issuer"]))
                builder.AddIdentityServer(new Uri(configuration["Authorize:Issuer"]), "SSO",
                    tags: new[] { "self" }, timeout: TimeSpan.FromSeconds(30));

            
            services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService),
                typeof(HealthCheckPublisherOptions).Assembly.GetType(
                    "Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckPublisherHostedService")));

            return builder;
        }
    }
}