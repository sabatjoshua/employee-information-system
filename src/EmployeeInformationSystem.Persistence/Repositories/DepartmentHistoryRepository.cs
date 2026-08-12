using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Persistence.Repositories
{
    public class DepartmentHistoryRepository : IDepartmentHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            DepartmentHistory history,
            CancellationToken cancellationToken = default)
        {
            await _context.DepartmentHistories.AddAsync(
                history,
                cancellationToken);
        }
    }
}
