using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.EmployeeRoles
{
    public sealed record GetEmployeeRoleByIdQuery(
        Guid EmployeeRoleId)
        : IRequest<GetEmployeeRoleByIdResponse?>;

    public sealed record GetEmployeeRoleByIdResponse(
        Guid Id,
        Guid EmployeeId,
        Guid RoleId,
        string StatusCode);

    public sealed class GetEmployeeRoleByIdHandler
        : IRequestHandler<
            GetEmployeeRoleByIdQuery,
            GetEmployeeRoleByIdResponse?>
    {
        private readonly IEmployeeRoleRepository _employeeRoleRepository;

        public GetEmployeeRoleByIdHandler(
            IEmployeeRoleRepository employeeRoleRepository)
        {
            _employeeRoleRepository = employeeRoleRepository;
        }

        public async Task<GetEmployeeRoleByIdResponse?> Handle(
            GetEmployeeRoleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var employeeRole = await _employeeRoleRepository.GetByIdAsync(
                request.EmployeeRoleId,
                cancellationToken);

            if (employeeRole is null)
            {
                return null;
            }

            return new GetEmployeeRoleByIdResponse(
                employeeRole.Id,
                employeeRole.EmployeeId,
                employeeRole.RoleId,
                employeeRole.StatusCode);
        }
    }
}