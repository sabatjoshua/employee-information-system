using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.EmployeeRoles
{
    public sealed record CreateEmployeeRoleCommand(
        Guid EmployeeId,
        Guid RoleId)
        : IRequest<CreateEmployeeRoleResponse>;

    public sealed record CreateEmployeeRoleResponse(
        Guid Id,
        Guid EmployeeId,
        Guid RoleId);

    public sealed class CreateEmployeeRoleHandler
        : IRequestHandler<CreateEmployeeRoleCommand, CreateEmployeeRoleResponse>
    {
        private readonly IEmployeeRoleRepository _employeeRoleRepository;
        private readonly IEmployeeRoleHistoryRepository _employeeRoleHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateEmployeeRoleHandler(
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

        public async Task<CreateEmployeeRoleResponse> Handle(
            CreateEmployeeRoleCommand command,
            CancellationToken cancellationToken)
        {
            var employeeRole = new EmployeeRole
            {
                EmployeeId = command.EmployeeId,
                RoleId = command.RoleId,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTimeOffset.UtcNow,
                StatusCode = StatusCodes.Active
            };

            var history = new EmployeeRoleHistory
            {
                EmployeeRoleId = employeeRole.Id,
                EmployeeId = employeeRole.EmployeeId,
                RoleId = employeeRole.RoleId,
                CreatedBy = employeeRole.CreatedBy,
                CreatedAt = employeeRole.CreatedAt,
                StatusCode = employeeRole.StatusCode,
                ActionTypeCode = ActionTypeCodes.Insert,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            await _employeeRoleRepository.AddAsync(
                employeeRole,
                cancellationToken);

            await _employeeRoleHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new CreateEmployeeRoleResponse(
                employeeRole.Id,
                employeeRole.EmployeeId,
                employeeRole.RoleId);
        }
    }
}