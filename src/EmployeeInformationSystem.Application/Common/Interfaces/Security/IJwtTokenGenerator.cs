
namespace EmployeeInformationSystem.Application.Common.Interfaces.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(
            Guid userId,
            Guid employeeId,
            string userName);
    }
}