using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Application.Features.Employees
{
    public sealed record DeleteEmployeeCommand(
        Guid EmployeeId,
        Guid DeletedBy);

    public sealed record DeleteEmployeeResponse(
        Guid EmployeeId,
        string StatusCode);

    public sealed class DeleteEmployeeHandler
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeHistoryRepository _employeeHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeHandler(
            IEmployeeRepository employeeRepository,
            IEmployeeHistoryRepository employeeHistoryRepository,
            IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _employeeHistoryRepository = employeeHistoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteEmployeeResponse?> HandleAsync(
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
                ActionBy = command.DeletedBy,
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