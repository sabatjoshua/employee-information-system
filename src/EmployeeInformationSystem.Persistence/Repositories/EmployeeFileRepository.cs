
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformationSystem.Persistence.Repositories
{
    public sealed class EmployeeFileRepository
        : IEmployeeFileRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeFileRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            EmployeeFile employeeFile,
            CancellationToken cancellationToken = default)
        {
            await _context.EmployeeFiles.AddAsync(
                employeeFile,
                cancellationToken);
        }
        public async Task<List<EmployeeFile>> GetByEmployeeIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default)
        {
            return await _context.EmployeeFiles
                .AsNoTracking()
                .Where(x =>
                    x.EmployeeId == employeeId &&
                    x.StatusCode == StatusCodes.Active)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }
        public async Task<EmployeeFile?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _context.EmployeeFiles
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == id &&
                         x.StatusCode == StatusCodes.Active,
                    cancellationToken);
        }

    }
}