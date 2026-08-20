using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Application.Features.Departments
{

    public sealed record DeleteDepartmentCommand(
        Guid DepartmentId)
        : IRequest<DeleteDepartmentResponse?>;

    public sealed record DeleteDepartmentResponse(
        Guid Id,
        string StatusCode);

    public sealed class DeleteDepartmentHandler
        : IRequestHandler<DeleteDepartmentCommand, DeleteDepartmentResponse?>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDepartmentHistoryRepository _departmentHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteDepartmentHandler(
            IDepartmentRepository departmentRepository,
            IDepartmentHistoryRepository departmentHistoryRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _departmentRepository = departmentRepository;
            _departmentHistoryRepository = departmentHistoryRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<DeleteDepartmentResponse?> Handle(
            DeleteDepartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.GetByIdAsync(
                command.DepartmentId,
                cancellationToken);

            if (department is null)
            {
                return null;
            }

            department.StatusCode = StatusCodes.Inactive;

            var history = new DepartmentHistory
            {
                DepartmentId = department.Id,
                Name = department.Name,
                CreatedBy = department.CreatedBy,
                CreatedAt = department.CreatedAt,
                StatusCode = department.StatusCode,
                ActionTypeCode = ActionTypeCodes.Delete,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };
            var updatedBy = department.UpdatedBy;
            var updatedAt = department.UpdatedAt;

            if (department.UpdatedBy.HasValue && department.UpdatedAt.HasValue)
            {
                history.SetUpdated(department.UpdatedBy.Value, department.UpdatedAt.Value);
            }

            await _departmentHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new DeleteDepartmentResponse(
                department.Id,
                department.StatusCode);
        }
    }
}
