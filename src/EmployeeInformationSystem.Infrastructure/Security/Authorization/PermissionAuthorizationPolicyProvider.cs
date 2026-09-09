using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace EmployeeInformationSystem.Infrastructure.Security.Authorization
{
    public sealed class PermissionAuthorizationPolicyProvider
        : DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(
            string policyName)
        {
            if (policyName.StartsWith(
                "Permission:",
                StringComparison.OrdinalIgnoreCase))
            {
                var functionCode = policyName["Permission:".Length..];

                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(
                        new PermissionRequirement(functionCode))
                    .Build();

                return policy;
            }

            return await base.GetPolicyAsync(policyName);
        }
    }
}