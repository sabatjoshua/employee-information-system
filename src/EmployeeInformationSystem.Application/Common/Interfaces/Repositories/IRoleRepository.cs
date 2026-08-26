using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(
            Guid roleId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Role role,
            CancellationToken cancellationToken = default);
    }
}