using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;

namespace EmployeeInformationSystem.Persistence.Repositories
{
    public sealed class EmployeeFileHistoryRepository
        : IEmployeeFileHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeFileHistoryRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            EmployeeFileHistory history,
            CancellationToken cancellationToken = default)
        {
            await _context.EmployeeFilesHistories.AddAsync(
                history,
                cancellationToken);
        }
    }
}