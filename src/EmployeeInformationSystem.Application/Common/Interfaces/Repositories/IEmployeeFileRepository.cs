using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IEmployeeFileRepository
    {
        Task AddAsync(
            EmployeeFile employeeFile,
            CancellationToken cancellationToken = default);

        Task<List<EmployeeFile>> GetByEmployeeIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default);
        Task<EmployeeFile?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}