using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using Merchant.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Merchant
{
   public static class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        private static string _environmentVariable;

        public static int Main(string[] args)
        {
            _environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_environmentVariable ?? "Production"}.json", optional: true)
                .AddJsonFile("appsettings.secrets.json", optional: true,reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            LogExtension.ConfigureLogging(Configuration, _environmentVariable);
            try
            {
                Log.Logger.Information(@$"Starting web host at {_environmentVariable} and current folder {Directory.GetCurrentDirectory()}" );
                CreateHostBuilder(args, Log.Logger).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, @$"Host terminated unexpectedly = {ex.Message} {ex.StackTrace}" );
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, ILogger logger) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog(logger)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                        {
                            if (_environmentVariable == "Development") return;
                            var sslConfig = Configuration.GetSection("Ssl");
                            var certPath = sslConfig["Path"];
                            var certPassword = sslConfig["Pass"];
                            var port = Convert.ToInt32(sslConfig["Port"]);
                            options.Listen(IPAddress.Any, port, listenOptions =>
                            {
                                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                                listenOptions.UseHttps(certPath, certPassword);
                            });
                        })
                        .UseStartup<Startup>();
                });
    }
}
