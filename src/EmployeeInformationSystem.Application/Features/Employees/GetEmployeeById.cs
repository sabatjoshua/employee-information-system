using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Employees
{
    public sealed record GetEmployeeByIdQuery(Guid EmployeeId)
    : IRequest<GetEmployeeByIdResponse?>;

    public sealed record GetEmployeeByIdResponse(
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

    public sealed class GetEmployeeByIdHandler
    : IRequestHandler<GetEmployeeByIdQuery, GetEmployeeByIdResponse?>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeeByIdHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<GetEmployeeByIdResponse?> Handle(
            GetEmployeeByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.GetByIdAsync(
                query.EmployeeId,
                cancellationToken);

            if (employee is null)
            {
                return null;
            }

            return new GetEmployeeByIdResponse(
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
                employee.PositionId);
        }
    }
}
