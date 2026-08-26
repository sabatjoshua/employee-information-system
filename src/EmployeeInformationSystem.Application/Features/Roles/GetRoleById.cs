using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Roles
{
    public sealed record GetRoleByIdQuery(Guid RoleId)
        : IRequest<GetRoleByIdResponse?>;

    public sealed record GetRoleByIdResponse(
        Guid Id,
        string Name,
        string? Description,
        string StatusCode);

    public sealed class GetRoleByIdHandler
        : IRequestHandler<GetRoleByIdQuery, GetRoleByIdResponse?>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRoleByIdHandler(
            IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<GetRoleByIdResponse?> Handle(
            GetRoleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(
                request.RoleId,
                cancellationToken);

            if (role is null)
            {
                return null;
            }

            return new GetRoleByIdResponse(
                role.Id,
                role.Name,
                role.Description,
                role.StatusCode);
        }
    }
}