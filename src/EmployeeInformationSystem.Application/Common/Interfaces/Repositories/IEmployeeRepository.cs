using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default);

        Task<Employee?> GetByEmployeeNoAsync(
            string employeeNo,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Employee employee,
            CancellationToken cancellationToken = default);
        Task<List<Employee>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            string? search,
            CancellationToken cancellationToken = default);
    }
}
