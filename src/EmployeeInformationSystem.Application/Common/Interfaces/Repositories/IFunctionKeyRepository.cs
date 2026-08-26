using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IFunctionKeyRepository
    {
        Task<FunctionKey?> GetByIdAsync(
            Guid functionKeyId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            FunctionKey functionKey,
            CancellationToken cancellationToken = default);
    }
}