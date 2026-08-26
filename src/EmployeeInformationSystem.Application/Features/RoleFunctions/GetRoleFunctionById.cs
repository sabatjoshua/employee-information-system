using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.RoleFunctions
{
    public sealed record GetRoleFunctionByIdQuery(
        Guid RoleFunctionId)
        : IRequest<GetRoleFunctionByIdResponse?>;

    public sealed record GetRoleFunctionByIdResponse(
        Guid Id,
        Guid RoleId,
        Guid FunctionKeyId,
        string StatusCode);

    public sealed class GetRoleFunctionByIdHandler
        : IRequestHandler<
            GetRoleFunctionByIdQuery,
            GetRoleFunctionByIdResponse?>
    {
        private readonly IRoleFunctionRepository _roleFunctionRepository;

        public GetRoleFunctionByIdHandler(
            IRoleFunctionRepository roleFunctionRepository)
        {
            _roleFunctionRepository = roleFunctionRepository;
        }

        public async Task<GetRoleFunctionByIdResponse?> Handle(
            GetRoleFunctionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var roleFunction = await _roleFunctionRepository.GetByIdAsync(
                request.RoleFunctionId,
                cancellationToken);

            if (roleFunction is null)
            {
                return null;
            }

            return new GetRoleFunctionByIdResponse(
                roleFunction.Id,
                roleFunction.RoleId,
                roleFunction.FunctionKeyId,
                roleFunction.StatusCode);
        }
    }
}