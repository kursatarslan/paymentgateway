using System.Reflection;
using IdentityServer.Context;
using IdentityServer.Extensions;
using IdentityServer.Models;
using IdentityServer.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer;
internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder,IConfiguration configuration)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();
        var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
        
        builder.Services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(builder.Configuration["DbConnectionString"]));
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
            })
            .AddDeveloperSigningCredential()
            .AddConfigurationStore(option =>
                option.ConfigureDbContext = builder => builder.UseNpgsql(configuration["DbConnectionString"], options =>
                    options.MigrationsAssembly(migrationsAssembly)))
            .AddOperationalStore(option =>
            {                    
                option.EnableTokenCleanup = true;
                option.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
                option.ConfigureDbContext = builder => builder.UseNpgsql(configuration["DbConnectionString"], options =>
                {

                    options.MigrationsAssembly(migrationsAssembly);
                });
            }).AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseApiExceptionHandling();
        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();
        DatabaseInitializer.Initialize(app);
        app.UseIdentityServer();
        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}