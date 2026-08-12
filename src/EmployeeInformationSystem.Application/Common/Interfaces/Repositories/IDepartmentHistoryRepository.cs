using EmployeeInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IDepartmentHistoryRepository
    {
        Task AddAsync(
            DepartmentHistory history,
            CancellationToken cancellationToken = default);
    }
}
