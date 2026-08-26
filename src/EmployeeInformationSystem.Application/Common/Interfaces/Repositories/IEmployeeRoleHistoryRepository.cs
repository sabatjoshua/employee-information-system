using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IEmployeeRoleHistoryRepository
    {
        Task AddAsync(
            EmployeeRoleHistory history,
            CancellationToken cancellationToken = default);
    }
}