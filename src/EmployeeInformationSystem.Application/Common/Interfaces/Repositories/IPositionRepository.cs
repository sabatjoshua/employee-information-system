using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IPositionRepository
    {
        Task<Position?> GetByIdAsync(
            Guid positionId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Position>> GetByDepartmentIdAsync(
            Guid departmentId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Position position,
            CancellationToken cancellationToken = default);
    }
}
