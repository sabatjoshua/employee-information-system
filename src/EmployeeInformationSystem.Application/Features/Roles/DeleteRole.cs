using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Roles
{
    public sealed record DeleteRoleCommand(
        Guid RoleId)
        : IRequest<DeleteRoleResponse?>;

    public sealed record DeleteRoleResponse(
        Guid Id,
        string Name,
        string? Description,
        string StatusCode);

    public sealed class DeleteRoleHandler
        : IRequestHandler<DeleteRoleCommand, DeleteRoleResponse?>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleHistoryRepository _roleHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteRoleHandler(
            IRoleRepository roleRepository,
            IRoleHistoryRepository roleHistoryRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _roleRepository = roleRepository;
            _roleHistoryRepository = roleHistoryRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<DeleteRoleResponse?> Handle(
            DeleteRoleCommand command,
            CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(
                command.RoleId,
                cancellationToken);

            if (role is null)
            {
                return null;
            }

            role.StatusCode = StatusCodes.Inactive;

            var history = new RoleHistory
            {
                RoleId = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedBy = role.CreatedBy,
                CreatedAt = role.CreatedAt,
                StatusCode = role.StatusCode,
                ActionTypeCode = ActionTypeCodes.Delete,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            if (role.UpdatedBy.HasValue &&
                role.UpdatedAt.HasValue)
            {
                history.SetUpdated(
                    role.UpdatedBy.Value,
                    role.UpdatedAt.Value);
            }

            await _roleHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new DeleteRoleResponse(
                role.Id,
                role.Name,
                role.Description,
                role.StatusCode);
        }
    }
}