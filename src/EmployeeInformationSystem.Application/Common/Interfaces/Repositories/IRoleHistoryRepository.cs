using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IRoleHistoryRepository
    {
        Task AddAsync(
            RoleHistory history,
            CancellationToken cancellationToken = default);
    }
}