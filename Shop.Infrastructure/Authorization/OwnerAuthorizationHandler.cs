using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Infrastructure.Authorization;

public class OwnerAuthorizationHandler : AuthorizationHandler<ResourceAuthorizationRequirement, Guid>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ResourceAuthorizationRequirement requirement, Guid userId)
    {
        var claimId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        
        if (claimId == userId.ToString()) context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}