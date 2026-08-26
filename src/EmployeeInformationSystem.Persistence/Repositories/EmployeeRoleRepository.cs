using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformationSystem.Persistence.Repositories
{
    public class EmployeeRoleRepository : IEmployeeRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeRole?> GetByIdAsync(
            Guid employeeRoleId,
            CancellationToken cancellationToken = default)
        {
            return await _context.EmployeeRoles
                .FirstOrDefaultAsync(
                    x => x.Id == employeeRoleId,
                    cancellationToken);
        }

        public async Task AddAsync(
            EmployeeRole employeeRole,
            CancellationToken cancellationToken = default)
        {
            await _context.EmployeeRoles.AddAsync(
                employeeRole,
                cancellationToken);
        }
    }
}