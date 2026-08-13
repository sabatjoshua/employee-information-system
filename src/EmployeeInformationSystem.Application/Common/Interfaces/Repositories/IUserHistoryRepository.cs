using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IUserHistoryRepository
    {
        Task AddAsync(
            UserHistory history,
            CancellationToken cancellationToken = default);
    }
}