using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IEmployeeRoleRepository
    {
        Task<EmployeeRole?> GetByIdAsync(
            Guid employeeRoleId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            EmployeeRole employeeRole,
            CancellationToken cancellationToken = default);
    }
}