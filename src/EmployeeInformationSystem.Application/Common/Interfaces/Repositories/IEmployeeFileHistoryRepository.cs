using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IEmployeeFileHistoryRepository
    {
        Task AddAsync(
            EmployeeFileHistory history,
            CancellationToken cancellationToken = default);
    }
}