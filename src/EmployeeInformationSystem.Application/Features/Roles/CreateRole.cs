using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Roles
{
    public sealed record CreateRoleCommand(
        string Name,
        string? Description)
        : IRequest<CreateRoleResponse>;

    public sealed record CreateRoleResponse(
        Guid Id,
        string Name,
        string? Description);

    public sealed class CreateRoleHandler
        : IRequestHandler<CreateRoleCommand, CreateRoleResponse>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleHistoryRepository _roleHistoryRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateRoleHandler(
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IRoleHistoryRepository roleHistoryRepository,
            ICurrentUserService currentUserService)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _roleHistoryRepository = roleHistoryRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CreateRoleResponse> Handle(
            CreateRoleCommand command,
            CancellationToken cancellationToken)
        {
            var role = new Role
            {
                Name = command.Name,
                Description = command.Description,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTimeOffset.UtcNow,
                StatusCode = StatusCodes.Active
            };

            var history = new RoleHistory
            {
                RoleId = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedBy = role.CreatedBy,
                CreatedAt = role.CreatedAt,
                StatusCode = role.StatusCode,
                ActionTypeCode = ActionTypeCodes.Insert,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            await _roleRepository.AddAsync(
                role,
                cancellationToken);

            await _roleHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new CreateRoleResponse(
                role.Id,
                role.Name,
                role.Description);
        }
    }
}