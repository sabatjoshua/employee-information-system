using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.EmployeeRoles
{
    public sealed record DeleteEmployeeRoleCommand(
        Guid EmployeeRoleId)
        : IRequest<DeleteEmployeeRoleResponse?>;

    public sealed record DeleteEmployeeRoleResponse(
        Guid Id,
        Guid EmployeeId,
        Guid RoleId,
        string StatusCode);

    public sealed class DeleteEmployeeRoleHandler
        : IRequestHandler<
            DeleteEmployeeRoleCommand,
            DeleteEmployeeRoleResponse?>
    {
        private readonly IEmployeeRoleRepository _employeeRoleRepository;
        private readonly IEmployeeRoleHistoryRepository _employeeRoleHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteEmployeeRoleHandler(
            IEmployeeRoleRepository employeeRoleRepository,
            IEmployeeRoleHistoryRepository employeeRoleHistoryRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _employeeRoleRepository = employeeRoleRepository;
            _employeeRoleHistoryRepository = employeeRoleHistoryRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<DeleteEmployeeRoleResponse?> Handle(
            DeleteEmployeeRoleCommand command,
            CancellationToken cancellationToken)
        {
            var employeeRole = await _employeeRoleRepository.GetByIdAsync(
                command.EmployeeRoleId,
                cancellationToken);

            if (employeeRole is null)
            {
                return null;
            }

            employeeRole.StatusCode = StatusCodes.Inactive;

            var history = new EmployeeRoleHistory
            {
                EmployeeRoleId = employeeRole.Id,
                EmployeeId = employeeRole.EmployeeId,
                RoleId = employeeRole.RoleId,
                CreatedBy = employeeRole.CreatedBy,
                CreatedAt = employeeRole.CreatedAt,
                StatusCode = employeeRole.StatusCode,
                ActionTypeCode = ActionTypeCodes.Delete,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            if (employeeRole.UpdatedBy.HasValue &&
                employeeRole.UpdatedAt.HasValue)
            {
                history.SetUpdated(
                    employeeRole.UpdatedBy.Value,
                    employeeRole.UpdatedAt.Value);
            }

            await _employeeRoleHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new DeleteEmployeeRoleResponse(
                employeeRole.Id,
                employeeRole.EmployeeId,
                employeeRole.RoleId,
                employeeRole.StatusCode);
        }
    }
}