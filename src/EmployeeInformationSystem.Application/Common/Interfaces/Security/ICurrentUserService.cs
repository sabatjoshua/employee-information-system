
namespace EmployeeInformationSystem.Application.Common.Interfaces.Security
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }

        Guid EmployeeId { get; }

        string UserName { get; }
    }
}