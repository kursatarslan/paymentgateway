using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityServer.Context;
using IdentityServer.Migrate;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer;

public class DatabaseInitializer
{
    public static void Initialize(IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()
                   ?.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            context.Database.EnsureCreated();
            InitializeTokenServerConfigurationDatabase(scope);

            context.SaveChanges();
        }
    }

    private static void InitializeTokenServerConfigurationDatabase(IServiceScope scope)
    {

        scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
        scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
        scope.ServiceProvider.GetRequiredService<AuthDbContext>().Database.Migrate();
        
        var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        if (!context.Clients.Any())
        {
            foreach (var client in Config.Clients)
            {
                context.Clients.Add(client.ToEntity());
            }
            context.SaveChanges();
        }

        if (!context.IdentityResources.Any())
        {
            foreach (var resource in Config.IdentityResources)
            {
                context.IdentityResources.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }

        if (!context.ApiResources.Any())
        {
            foreach (var resource in Config.ApiResources)
            {
                context.ApiResources.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }
        
        if (!context.ApiScopes.Any())
        {
            foreach (var resource in Config.ApiScopes)
            {
                context.ApiScopes.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        if (!userManager.Users.Any())
        {
            foreach (var testUser in Config.GetUsers)
            {
                var identityUser = new ApplicationUser(testUser.Username)
                {
                    Id = testUser.SubjectId,
                    Email = testUser.Username,
                    EmailConfirmed = true,
                    
                };
                userManager.CreateAsync(identityUser, "Password123!").Wait();
                userManager.AddClaimsAsync(identityUser, testUser.Claims.ToList()).Wait();
            }
        }
    }
}