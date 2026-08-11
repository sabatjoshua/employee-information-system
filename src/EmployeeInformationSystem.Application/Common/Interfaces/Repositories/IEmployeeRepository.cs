using EmployeeInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        void Update(Employee employee);

        void Remove(Employee employee);
    }
}
