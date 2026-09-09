using Microsoft.AspNetCore.Authorization;

namespace EmployeeInformationSystem.Infrastructure.Security.Authorization
{
    public sealed class PermissionRequirement
        : IAuthorizationRequirement
    {
        public string FunctionCode { get; }

        public PermissionRequirement(string functionCode)
        {
            FunctionCode = functionCode;
        }
    }
}