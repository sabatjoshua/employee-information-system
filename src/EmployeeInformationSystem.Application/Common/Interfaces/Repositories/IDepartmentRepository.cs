using EmployeeInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{

    public interface IDepartmentRepository
    {
        Task<Department?> GetByIdAsync(
            Guid departmentId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Department department,
            CancellationToken cancellationToken = default);
    }
}
