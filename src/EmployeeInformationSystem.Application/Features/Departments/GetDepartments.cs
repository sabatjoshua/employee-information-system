using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Departments
{
    public sealed record GetDepartmentsQuery
        : IRequest<IReadOnlyList<GetDepartmentsResponse>>;

    public sealed record GetDepartmentsResponse(
        Guid Id,
        string Name);

    public sealed class GetDepartmentsHandler
        : IRequestHandler<
            GetDepartmentsQuery,
            IReadOnlyList<GetDepartmentsResponse>>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentsHandler(
            IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IReadOnlyList<GetDepartmentsResponse>> Handle(
            GetDepartmentsQuery request,
            CancellationToken cancellationToken)
        {
            var departments = await _departmentRepository.GetAllAsync(
                cancellationToken);

            return departments
                .Select(department => new GetDepartmentsResponse(
                    department.Id,
                    department.Name))
                .ToList();
        }
    }
}