using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;

namespace EmployeeInformationSystem.Persistence.Repositories
{
    public class RoleHistoryRepository : IRoleHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            RoleHistory history,
            CancellationToken cancellationToken = default)
        {
            await _context.RoleHistories.AddAsync(
                history,
                cancellationToken);
        }
    }
}