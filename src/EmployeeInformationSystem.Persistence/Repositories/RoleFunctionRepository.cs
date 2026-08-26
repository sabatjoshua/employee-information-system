using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformationSystem.Persistence.Repositories
{
    public class RoleFunctionRepository : IRoleFunctionRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleFunctionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RoleFunction?> GetByIdAsync(
            Guid roleFunctionId,
            CancellationToken cancellationToken = default)
        {
            return await _context.RoleFunctions
                .FirstOrDefaultAsync(
                    x => x.Id == roleFunctionId,
                    cancellationToken);
        }

        public async Task AddAsync(
            RoleFunction roleFunction,
            CancellationToken cancellationToken = default)
        {
            await _context.RoleFunctions.AddAsync(
                roleFunction,
                cancellationToken);
        }
    }
}