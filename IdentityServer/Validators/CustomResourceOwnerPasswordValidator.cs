using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using IdentityServer.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Validators;


public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly ILogger<CustomResourceOwnerPasswordValidator> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private IEventService _events;

    public CustomResourceOwnerPasswordValidator(ILogger<CustomResourceOwnerPasswordValidator> logger,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IEventService events)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _events = events;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var user = await _userManager.FindByNameAsync(context.UserName);
        var errorMessage = "";
        if (user != null)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, true);
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogInformation("You need to confirm your email. {username}", context.UserName);
                await Task.CompletedTask;
                return;
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogInformation("You need to Lockout. {username}", context.UserName);
                await Task.CompletedTask;
                return;
            }
            if (result.Succeeded)
            {
                var sub = await _userManager.GetUserIdAsync(user);

                _logger.LogInformation("Credentials validated for username: {username}", context.UserName);
                var claims = GetUsersCustom(user).ToList();
                foreach (var claim in claims)
                {
                    context.Request.ClientClaims.Add(claim);
                }  
                context.Result = new GrantValidationResult(sub, OidcConstants.AuthenticationMethods.Password, claims);
              
                 await Task.CompletedTask;
                 return;
            }
            if (result.IsLockedOut)
            {
                errorMessage = $"Authentication failed for username: {context.UserName}, reason: locked out";
                _logger.LogInformation("Authentication failed for username: {username}, reason: locked out", context.UserName);
            }
            else if (result.IsNotAllowed)
            {
                errorMessage = $"Authentication failed for username: {context.UserName}, reason: not allowed";
                _logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.UserName);
            }
            else
            {
                errorMessage = $"Authentication failed for username: {context.UserName},reason: invalid credentials";
                _logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
            }
        }
        else
        {
            errorMessage = $"No user found matching username: {context.UserName}";
            _logger.LogInformation("No user found matching username: {username}", context.UserName);
        }

        await _events.RaiseAsync(new MissingEvent(context.UserName, errorMessage, interactive: false));
        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, errorMessage);
        await Task.CompletedTask;
    }
    
    private IEnumerable<Claim> GetUsersCustom(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            { new Claim(JwtClaimTypes.Id, user.Id.ToString()) },
            { new Claim(JwtClaimTypes.PreferredUserName, user.UserName) },
            { new Claim(JwtClaimTypes.Email, user.Email) }
        };
        return claims;
    }
}
