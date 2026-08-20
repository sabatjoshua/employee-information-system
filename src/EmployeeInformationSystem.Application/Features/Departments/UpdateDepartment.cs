using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Departments
{
    public sealed record UpdateDepartmentCommand(
        Guid DepartmentId,
        string Name)
        : IRequest<UpdateDepartmentResponse?>;

    public sealed record UpdateDepartmentResponse(
        Guid Id,
        string Name,
        string StatusCode);

    public sealed class UpdateDepartmentHandler
        : IRequestHandler<UpdateDepartmentCommand, UpdateDepartmentResponse?>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDepartmentHistoryRepository _departmentHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateDepartmentHandler(
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

        public async Task<UpdateDepartmentResponse?> Handle(
            UpdateDepartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.GetByIdAsync(
                command.DepartmentId,
                cancellationToken);

            if (department is null)
            {
                return null;
            }

            department.Name = command.Name;
            department.SetUpdated(_currentUserService.UserId, DateTimeOffset.UtcNow);

            var history = new DepartmentHistory
            {
                DepartmentId = department.Id,
                Name = department.Name,
                CreatedBy = department.CreatedBy,
                CreatedAt = department.CreatedAt,
                StatusCode = department.StatusCode,
                ActionTypeCode = ActionTypeCodes.Update,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            if (department.UpdatedBy.HasValue && department.UpdatedAt.HasValue)
            {
                history.SetUpdated(department.UpdatedBy.Value, department.UpdatedAt.Value);
            }

            await _departmentHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new UpdateDepartmentResponse(
                department.Id,
                department.Name,
                department.StatusCode);
        }
    }
}
