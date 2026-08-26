using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IRoleFunctionHistoryRepository
    {
        Task AddAsync(
            RoleFunctionHistory history,
            CancellationToken cancellationToken = default);
    }
}