using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{

    public interface IDepartmentRepository
    {
        Task<Department?> GetByIdAsync(
            Guid departmentId,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Department>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Department department,
            CancellationToken cancellationToken = default);
    }
}
