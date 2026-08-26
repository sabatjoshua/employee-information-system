using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IFunctionKeyHistoryRepository
    {
        Task AddAsync(
            FunctionKeyHistory history,
            CancellationToken cancellationToken = default);
    }
}