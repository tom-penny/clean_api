using Microsoft.AspNetCore.Authorization;

namespace Shop.Infrastructure.Authorization;

public class AdminAuthorizationHandler : AuthorizationHandler<ResourceAuthorizationRequirement, Guid>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ResourceAuthorizationRequirement requirement, Guid userId)
    {
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}