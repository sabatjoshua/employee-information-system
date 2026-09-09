using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformationSystem.Persistence.Security
{
    public sealed class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public PermissionService(
            ApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<bool> HasPermissionAsync(
            string functionCode,
            CancellationToken cancellationToken = default)
        {
            var employeeId = _currentUserService.EmployeeId;

            if (employeeId == Guid.Empty)
            {
                return false;
            }

            return await (
                from employeeRole in _context.EmployeeRoles
                join roleFunction in _context.RoleFunctions
                    on employeeRole.RoleId equals roleFunction.RoleId
                join functionKey in _context.FunctionKeys
                    on roleFunction.FunctionKeyId equals functionKey.Id
                where employeeRole.EmployeeId == employeeId
                    && functionKey.FunctionCode == functionCode
                select functionKey.Id
            ).AnyAsync(cancellationToken);
        }
    }
}