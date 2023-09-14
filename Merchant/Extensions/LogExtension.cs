using System;
using System.IO;
using Elastic.Apm.SerilogEnricher;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;

namespace Merchant.Extensions
{
    public static class LogExtension
    {
        public static void ConfigureLogging(IConfiguration configuration, string environmentVariable)
        {
            var logFileName = configuration["ApplicationLogFileName"] ?? "ApplicationLog";
            var applicationName = configuration["ElasticConfiguration:ApplicationName"] ?? "AppName Not Set";

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionData()
                .Enrich.WithDemystifiedStackTraces()
                .Enrich.WithMachineName()
                .Enrich.WithClientIp()
                .Enrich.WithHttpRequestId()
                .Enrich.WithUserName()
                .Enrich.WithExceptionDetails()
                .Enrich.WithElasticApmCorrelationInfo()
                .Enrich.WithClientAgent()
                .Enrich.WithProperty("Environment", environmentVariable)
                .Enrich.WithProperty("ApplicationName", applicationName)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Environment} {Message:lj}{NewLine}")
                .WriteTo.File(
                    new JsonFormatter(renderMessage: true),
                    Path.Combine(@$"Logs/{logFileName}.log"), rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                .CreateLogger();

        }
    }
}