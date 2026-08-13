using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Employees
{
    public sealed record UpdateEmployeeCommand(
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
        Guid PositionId,
        Guid UpdatedBy)
    : IRequest<UpdateEmployeeResponse?>;

    public sealed record UpdateEmployeeResponse(
        Guid EmployeeId,
        string EmployeeNo,
        string FirstName,
        string? MiddleName,
        string LastName,
        string StatusCode);

    public sealed class UpdateEmployeeHandler
    : IRequestHandler<UpdateEmployeeCommand, UpdateEmployeeResponse?>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeHistoryRepository _employeeHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeHandler(
            IEmployeeRepository employeeRepository,
            IEmployeeHistoryRepository employeeHistoryRepository,
            IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _employeeHistoryRepository = employeeHistoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateEmployeeResponse?> Handle(
            UpdateEmployeeCommand command,
            CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.GetByIdAsync(
                command.EmployeeId,
                cancellationToken);

            if (employee is null)
            {
                return null;
            }

            employee.EmployeeNo = command.EmployeeNo;
            employee.FirstName = command.FirstName;
            employee.MiddleName = command.MiddleName;
            employee.LastName = command.LastName;
            employee.GenderCode = command.GenderCode;
            employee.BirthDate = command.BirthDate;
            employee.Email = command.Email;
            employee.MobileNo = command.MobileNo;
            employee.HireDate = command.HireDate;
            employee.DepartmentId = command.DepartmentId;
            employee.PositionId = command.PositionId;

            employee.SetUpdated(
                command.UpdatedBy,
                DateTimeOffset.UtcNow);

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
                ActionTypeCode = ActionTypeCodes.Update,
                ActionBy = command.UpdatedBy,
                ActionAt = DateTimeOffset.UtcNow
            };

            if (employee.UpdatedBy.HasValue &&
                employee.UpdatedAt.HasValue)
            {
                history.SetUpdated(
                    employee.UpdatedBy.Value,
                    employee.UpdatedAt.Value);
            }

            await _employeeHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new UpdateEmployeeResponse(
                employee.Id,
                employee.EmployeeNo,
                employee.FirstName,
                employee.MiddleName,
                employee.LastName,
                employee.StatusCode);
        }
    }
}