using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformationSystem.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetByIdAsync(
            Guid roleId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(
                    x => x.Id == roleId,
                    cancellationToken);
        }

        public async Task AddAsync(
            Role role,
            CancellationToken cancellationToken = default)
        {
            await _context.Roles.AddAsync(
                role,
                cancellationToken);
        }
    }
}