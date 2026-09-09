using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeInformationSystem.Infrastructure.Security.Authorization
{
    public sealed class PermissionAuthorizationHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionService _permissionService;

        public PermissionAuthorizationHandler(
            IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var hasPermission =
                await _permissionService.HasPermissionAsync(
                    requirement.FunctionCode);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}