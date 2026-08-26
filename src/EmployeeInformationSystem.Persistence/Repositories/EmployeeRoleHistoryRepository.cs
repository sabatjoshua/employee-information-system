using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;

namespace EmployeeInformationSystem.Persistence.Repositories
{
    public class EmployeeRoleHistoryRepository : IEmployeeRoleHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRoleHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            EmployeeRoleHistory history,
            CancellationToken cancellationToken = default)
        {
            await _context.EmployeeRoleHistories.AddAsync(
                history,
                cancellationToken);
        }
    }
}