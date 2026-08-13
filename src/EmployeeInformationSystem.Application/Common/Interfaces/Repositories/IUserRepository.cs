using EmployeeInformationSystem.Domain.Entities;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(
            User user,
            CancellationToken cancellationToken = default);

        Task<User?> GetByIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
        Task<User?> GetByUserNameAsync(
            string userName,
            CancellationToken cancellationToken = default);
    }
}