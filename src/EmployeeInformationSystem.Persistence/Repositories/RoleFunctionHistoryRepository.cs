using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;

namespace EmployeeInformationSystem.Persistence.Repositories
{
    public class RoleFunctionHistoryRepository : IRoleFunctionHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleFunctionHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            RoleFunctionHistory history,
            CancellationToken cancellationToken = default)
        {
            await _context.RoleFunctionHistories.AddAsync(
                history,
                cancellationToken);
        }
    }
}