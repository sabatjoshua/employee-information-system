using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Employees
{
    public sealed record CreateEmployeeCommand(
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
        Guid PositionId,
        Guid CreatedBy)
    : IRequest<CreateEmployeeResponse>;

    public sealed record CreateEmployeeResponse(
        Guid Id,
        string EmployeeNo,
        string FirstName,
        string LastName);

    public sealed class CreateEmployeeHandler
    : IRequestHandler<CreateEmployeeCommand, CreateEmployeeResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeHistoryRepository _employeeHistoryRepository;

        public CreateEmployeeHandler(
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork,
            IEmployeeHistoryRepository employeeHistoryRepository)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _employeeHistoryRepository = employeeHistoryRepository;
        }

        public async Task<CreateEmployeeResponse> Handle(
            CreateEmployeeCommand command,
            CancellationToken cancellationToken = default)
        {
            var employee = new Employee
            {
                EmployeeNo = command.EmployeeNo,
                FirstName = command.FirstName,
                MiddleName = command.MiddleName,
                LastName = command.LastName,
                GenderCode = command.GenderCode,
                BirthDate = command.BirthDate,
                Email = command.Email,
                MobileNo = command.MobileNo,
                HireDate = command.HireDate,
                DepartmentId = command.DepartmentId,
                PositionId = command.PositionId,
                CreatedBy = command.CreatedBy,
                CreatedAt = DateTimeOffset.UtcNow,
                StatusCode = StatusCodes.Active
            };

            var history = new EmployeeHistory
            {
                EmployeeId = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                GenderCode = employee.GenderCode,
                BirthDate = employee.BirthDate,
                Email = employee.Email,
                MobileNo = employee.MobileNo,
                HireDate = employee.HireDate,
                DepartmentId = employee.DepartmentId,
                PositionId = employee.PositionId,
                CreatedBy = employee.CreatedBy,
                CreatedAt = employee.CreatedAt,
                StatusCode = employee.StatusCode,
                ActionTypeCode = ActionTypeCodes.Insert,
                ActionBy = command.CreatedBy,
                ActionAt = DateTimeOffset.UtcNow
            };

            await _employeeRepository.AddAsync(
                employee,
                cancellationToken);

            await _employeeHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new CreateEmployeeResponse(
                employee.Id,
                employee.EmployeeNo,
                employee.FirstName,
                employee.LastName);
        }
    }
}