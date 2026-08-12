using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Application.Features.Departments
{
    public sealed record GetDepartmentByIdQuery(Guid DepartmentId);

    public sealed record GetDepartmentByIdResponse(
        Guid Id,
        string Name,
        string StatusCode);

    public sealed class GetDepartmentByIdHandler
    {
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentByIdHandler(
            IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<GetDepartmentByIdResponse?> HandleAsync(
            GetDepartmentByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.GetByIdAsync(
                query.DepartmentId,
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
