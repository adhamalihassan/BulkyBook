using Microsoft.AspNetCore.Authorization;

namespace BulkyBook.Auth;

public class RequireCustomClaimHandler : AuthorizationHandler<RequireCustomClaimRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequireCustomClaimRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == requirement.ClaimType))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}