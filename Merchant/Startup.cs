using System.Net;
using Elastic.Apm.NetCoreAll;
using Merchant.Hubs;
using Merchant.Extensions;
using Merchant.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Merchant
{
    public class Startup
    {
        private readonly string _spaSourcePath;
        private readonly string _corsPolicyName;
        private readonly IConfiguration _configuration;
        public ReactSettings ReactSettings { get; set; } = new ReactSettings();

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _spaSourcePath = _configuration.GetValue<string>("SPA:SourcePath");
            _corsPolicyName = _configuration.GetValue<string>("CORS:PolicyName");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add CORS
            services.AddCorsConfig(_corsPolicyName);

            // Register RazorPages/Controllers
            services.AddControllers();

            // Add Brotli/Gzip response compression (prod only)
            services.AddResponseCompressionConfig(_configuration);
            
            #region HealthCheck
            services.AddDefaultHealthChecks(_configuration);
            #endregion
            services.Configure<AuthOptions>(_configuration.GetSection("Authorize"));
            // Add SignalR
            services.AddSignalR();
            // add react environment setting
            services.Configure<ReactSettings>(_configuration.GetSection("ReactSettings"));
            _configuration.GetSection("ReactSettings").Bind(ReactSettings);
            services.AddMvc(opt => opt.SuppressAsyncSuffixInActionNames = false);

            services.AddReactEnvironment($"{_spaSourcePath}/build", ReactSettings);

            // Register the Swagger services (using OpenApi 3.0)
            services.AddOpenApiDocument(configure => configure.Title = $"{this.GetType().Namespace} API");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // If development, enable Hot Module Replacement
            // If production, enable Brotli/Gzip response compression & strict transport security headers
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseResponseCompression();
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                // Do work that doesn't write to the Response.
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });
            app.UseSerilogRequestLogging();
            app.UseCors(_corsPolicyName);
            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/docs";
                settings.DocumentPath = "/docs/api-specification.json";
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();

            // Map controllers / SignalR hubs / HealthChecks
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
                endpoints.MapHub<UsersHub>("/hubs/users");
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = _spaSourcePath;

                if (env.IsDevelopment())
                    spa.UseReactDevelopmentServer(npmScript: "start");
            });
        }
    }
}
