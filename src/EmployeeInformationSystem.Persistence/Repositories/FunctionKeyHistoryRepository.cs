using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;

namespace EmployeeInformationSystem.Persistence.Repositories
{
    public class FunctionKeyHistoryRepository : IFunctionKeyHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public FunctionKeyHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            FunctionKeyHistory history,
            CancellationToken cancellationToken = default)
        {
            await _context.FunctionKeyHistories.AddAsync(
                history,
                cancellationToken);
        }
    }
}