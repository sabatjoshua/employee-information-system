using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.EmployeeRoles
{
    public sealed record UpdateEmployeeRoleCommand(
        Guid EmployeeRoleId,
        Guid EmployeeId,
        Guid RoleId)
        : IRequest<UpdateEmployeeRoleResponse?>;

    public sealed record UpdateEmployeeRoleResponse(
        Guid Id,
        Guid EmployeeId,
        Guid RoleId,
        string StatusCode);

    public sealed class UpdateEmployeeRoleHandler
        : IRequestHandler<
            UpdateEmployeeRoleCommand,
            UpdateEmployeeRoleResponse?>
    {
        private readonly IEmployeeRoleRepository _employeeRoleRepository;
        private readonly IEmployeeRoleHistoryRepository _employeeRoleHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateEmployeeRoleHandler(
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

        public async Task<UpdateEmployeeRoleResponse?> Handle(
            UpdateEmployeeRoleCommand command,
            CancellationToken cancellationToken)
        {
            var employeeRole = await _employeeRoleRepository.GetByIdAsync(
                command.EmployeeRoleId,
                cancellationToken);

            if (employeeRole is null)
            {
                return null;
            }

            employeeRole.EmployeeId = command.EmployeeId;
            employeeRole.RoleId = command.RoleId;

            employeeRole.SetUpdated(
                _currentUserService.UserId,
                DateTimeOffset.UtcNow);

            var history = new EmployeeRoleHistory
            {
                EmployeeRoleId = employeeRole.Id,
                EmployeeId = employeeRole.EmployeeId,
                RoleId = employeeRole.RoleId,
                CreatedBy = employeeRole.CreatedBy,
                CreatedAt = employeeRole.CreatedAt,
                StatusCode = employeeRole.StatusCode,
                ActionTypeCode = ActionTypeCodes.Update,
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

            return new UpdateEmployeeRoleResponse(
                employeeRole.Id,
                employeeRole.EmployeeId,
                employeeRole.RoleId,
                employeeRole.StatusCode);
        }
    }
}