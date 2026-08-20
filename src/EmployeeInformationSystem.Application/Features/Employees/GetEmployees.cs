using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Employees
{
    public sealed record GetEmployeesQuery
        : IRequest<List<GetEmployeesResponse>>;

    public sealed record GetEmployeesResponse(
        Guid EmployeeId,
        string EmployeeNo,
        string FirstName,
        string? MiddleName,
        string LastName,
        string GenderCode,
        DateTimeOffset BirthDate,
        string? Email,
        string? MobileNo,
        DateTimeOffset HireDate,
        Guid DepartmentId,
        Guid PositionId);

    public sealed class GetEmployeesHandler
        : IRequestHandler<GetEmployeesQuery, List<GetEmployeesResponse>>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeesHandler(
            IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<GetEmployeesResponse>> Handle(
            GetEmployeesQuery query,
            CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetAllAsync(
                cancellationToken);

            return employees
                .Select(employee => new GetEmployeesResponse(
                    employee.Id,
                    employee.EmployeeNo,
                    employee.FirstName,
                    employee.MiddleName,
                    employee.LastName,
                    employee.GenderCode,
                    employee.BirthDate,
                    employee.Email,
                    employee.MobileNo,
                    employee.HireDate,
                    employee.DepartmentId,
                    employee.PositionId))
                .ToList();
        }
    }
}