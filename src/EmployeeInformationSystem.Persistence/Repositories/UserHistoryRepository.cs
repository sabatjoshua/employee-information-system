using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;

namespace EmployeeInformationSystem.Persistence.Repositories
{
    public class UserHistoryRepository : IUserHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public UserHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            UserHistory history,
            CancellationToken cancellationToken = default)
        {
            await _context.UserHistories.AddAsync(
                history,
                cancellationToken);
        }
    }
}