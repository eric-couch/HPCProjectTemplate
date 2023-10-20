using Microsoft.AspNetCore.Identity;
using HPCProjectTemplate.Server.Models;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace HPCProjectTemplate.Server.Factory;

public class CustomClaimsFactory : UserClaimsPrincipalFactory<ApplicationUser>
{
    public CustomClaimsFactory( UserManager<ApplicationUser> userManager, 
                                IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
    {
        
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim("role", UserManager.GetRolesAsync(user).Result.FirstOrDefault() ?? ""));
        identity.AddClaim(new Claim("FirstName", user.FirstName ?? ""));
        identity.AddClaim(new Claim("LastName", user.LastName ?? ""));
        return identity;
    }
}
