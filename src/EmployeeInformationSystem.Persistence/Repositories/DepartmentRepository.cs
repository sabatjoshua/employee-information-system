using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformationSystem.Persistence.Repositories
{

    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Department?> GetByIdAsync(
            Guid departmentId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(
                    x => x.Id == departmentId,
                    cancellationToken);
        }
        public async Task<IReadOnlyList<Department>> GetAllAsync(
            CancellationToken cancellationToken = default)
                {
                    return await _context.Departments
                        .OrderBy(x => x.Name)
                        .ToListAsync(cancellationToken);
                }

        public async Task AddAsync(
            Department department,
            CancellationToken cancellationToken = default)
        {
            await _context.Departments.AddAsync(
                department,
                cancellationToken);
        }
    }
}
