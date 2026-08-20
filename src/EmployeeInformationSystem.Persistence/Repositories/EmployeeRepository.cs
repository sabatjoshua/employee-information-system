using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformationSystem.Persistence.Repositories
{

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee?> GetByIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(
                    x => x.Id == employeeId,
                    cancellationToken);
        }

        public async Task<Employee?> GetByEmployeeNoAsync(
            string employeeNo,
            CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(
                    x => x.EmployeeNo == employeeNo,
                    cancellationToken);
        }

        public async Task AddAsync(
            Employee employee,
            CancellationToken cancellationToken = default)
        {
            await _context.Employees.AddAsync(
                employee, 
                cancellationToken);
        }
        public async Task<List<Employee>> GetAllAsync(
        CancellationToken cancellationToken = default)
            {
                return await _context.Employees
                    .AsNoTracking()
                    .Where(x => x.StatusCode == StatusCodes.Active)
                    .ToListAsync(cancellationToken);
            }
    }
}
