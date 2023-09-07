using Microsoft.AspNetCore.Authorization;

namespace BulkyBook.Auth;

public class RequireCustomClaimRequirement : IAuthorizationRequirement
{
    public string ClaimType { get; }

    public RequireCustomClaimRequirement(string claimType)
    {
        ClaimType = claimType;
    }
}