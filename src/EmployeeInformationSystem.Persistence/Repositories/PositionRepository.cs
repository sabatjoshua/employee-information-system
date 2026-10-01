using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformationSystem.Persistence.Repositories
{

    public class PositionRepository : IPositionRepository
    {
        private readonly ApplicationDbContext _context;

        public PositionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Position?> GetByIdAsync(
            Guid positionId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Positions
                .FirstOrDefaultAsync(
                    x => x.Id == positionId,
                    cancellationToken);
        }
        public async Task<IReadOnlyList<Position>> GetByDepartmentIdAsync(
            Guid departmentId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Positions
                .Where(x => x.DepartmentId == departmentId)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(
            Position position,
            CancellationToken cancellationToken = default)
        {
            await _context.Positions.AddAsync(
                position,
                cancellationToken);
        }
    }
}
