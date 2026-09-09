using Microsoft.AspNetCore.Authorization;

namespace EmployeeInformationSystem.API.Authorization
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public const string PolicyPrefix = "Permission:";

        public HasPermissionAttribute(string functionCode)
        {
            Policy = $"{PolicyPrefix}{functionCode}";
        }
    }
}