using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Employees
{
    public sealed record GetEmployeesQuery(
    int PageNumber,
    int PageSize,
    string? Search)
    : IRequest<GetEmployeesPagedResponse>;

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
    public sealed record GetEmployeesPagedResponse(
    List<GetEmployeesResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);

    public sealed class GetEmployeesHandler
    : IRequestHandler<GetEmployeesQuery, GetEmployeesPagedResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeesHandler(
            IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<GetEmployeesPagedResponse> Handle(
            GetEmployeesQuery query,
            CancellationToken cancellationToken)
        {
            var pageNumber = query.PageNumber < 1
                ? 1
                : query.PageNumber;

            var pageSize = query.PageSize < 1
                ? 10
                : query.PageSize;

            pageSize = Math.Min(pageSize, 100);

            var employees = await _employeeRepository.GetPagedAsync(
                pageNumber,
                pageSize,
                query.Search,
                cancellationToken);

            var totalCount = await _employeeRepository.CountAsync(
                query.Search,
                cancellationToken);

            var items = employees
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

            var totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize);

            return new GetEmployeesPagedResponse(
                items,
                pageNumber,
                pageSize,
                totalCount,
                totalPages);
        }
    }
}