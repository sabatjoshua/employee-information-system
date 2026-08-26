using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IRoleFunctionRepository
    {
        Task<RoleFunction?> GetByIdAsync(
            Guid roleFunctionId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            RoleFunction roleFunction,
            CancellationToken cancellationToken = default);
    }
}