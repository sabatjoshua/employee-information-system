namespace EmployeeInformationSystem.Application.Common.Interfaces.Security
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(
            string functionCode,
            CancellationToken cancellationToken = default);
    }
}