using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once EmptyNamespace
namespace IdentityServer.Models;

public class ApplicationUser : IdentityUser
{

    public ApplicationUser(): base()
    {
    }
    
    public ApplicationUser(string username) :base(username)
    {
            
    }
}

