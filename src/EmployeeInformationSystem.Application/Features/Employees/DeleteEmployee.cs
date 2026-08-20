using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Employees
{
    public sealed record DeleteEmployeeCommand(
        Guid EmployeeId)
    : IRequest<DeleteEmployeeResponse?>;

    public sealed record DeleteEmployeeResponse(
        Guid EmployeeId,
        string StatusCode);

    public sealed class DeleteEmployeeHandler
    : IRequestHandler<DeleteEmployeeCommand, DeleteEmployeeResponse?>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeHistoryRepository _employeeHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteEmployeeHandler(
            IEmployeeRepository employeeRepository,
            IEmployeeHistoryRepository employeeHistoryRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _employeeRepository = employeeRepository;
            _employeeHistoryRepository = employeeHistoryRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<DeleteEmployeeResponse?> Handle(
            DeleteEmployeeCommand command,
            CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.GetByIdAsync(
                command.EmployeeId,
                cancellationToken);

            if (employee is null)
            {
                return null;
            }

            employee.StatusCode = StatusCodes.Inactive;

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
                ActionTypeCode = ActionTypeCodes.Delete,
                ActionBy = _currentUserService.UserId,
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

            return new DeleteEmployeeResponse(
                employee.Id,
                employee.StatusCode);
        }
    }
}