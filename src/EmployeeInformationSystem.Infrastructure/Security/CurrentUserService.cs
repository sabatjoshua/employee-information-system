using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using Microsoft.AspNetCore.Http;

namespace EmployeeInformationSystem.Infrastructure.Security
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId =>
            GetGuidClaim("userId");

        public Guid EmployeeId =>
            GetGuidClaim("employeeId");

        public string UserName =>
            _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue("userName")
            ?? string.Empty;

        private Guid GetGuidClaim(string claimType)
        {
            var value = _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(claimType);

            return Guid.TryParse(value, out var result)
                ? result
                : Guid.Empty;
        }
    }
}