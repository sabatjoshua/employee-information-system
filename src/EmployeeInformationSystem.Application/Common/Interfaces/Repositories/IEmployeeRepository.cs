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
        Task<List<Employee>> GetAllAsync(
            CancellationToken cancellationToken = default);
    }
}
