using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Departments
{
    public sealed record GetDepartmentByIdQuery(Guid DepartmentId)
    : IRequest<GetDepartmentByIdResponse?>;

    public sealed record GetDepartmentByIdResponse(
        Guid Id,
        string Name,
        string StatusCode);

    public sealed class GetDepartmentByIdHandler
    : IRequestHandler<GetDepartmentByIdQuery, GetDepartmentByIdResponse?>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentByIdHandler(
            IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<GetDepartmentByIdResponse?> Handle(
        GetDepartmentByIdQuery request,
        CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByIdAsync(
                request.DepartmentId,
                cancellationToken);

            if (department is null)
            {
                return null;
            }

            return new GetDepartmentByIdResponse(
                department.Id,
                department.Name,
                department.StatusCode);
        }
    }
}
