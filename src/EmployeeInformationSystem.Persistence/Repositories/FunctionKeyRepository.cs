using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformationSystem.Persistence.Repositories
{
    public class FunctionKeyRepository : IFunctionKeyRepository
    {
        private readonly ApplicationDbContext _context;

        public FunctionKeyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FunctionKey?> GetByIdAsync(
            Guid functionKeyId,
            CancellationToken cancellationToken = default)
        {
            return await _context.FunctionKeys
                .FirstOrDefaultAsync(
                    x => x.Id == functionKeyId,
                    cancellationToken);
        }

        public async Task AddAsync(
            FunctionKey functionKey,
            CancellationToken cancellationToken = default)
        {
            await _context.FunctionKeys.AddAsync(
                functionKey,
                cancellationToken);
        }
    }
}