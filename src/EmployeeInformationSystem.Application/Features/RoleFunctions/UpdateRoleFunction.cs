using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.RoleFunctions
{
    public sealed record UpdateRoleFunctionCommand(
        Guid RoleFunctionId,
        Guid RoleId,
        Guid FunctionKeyId)
        : IRequest<UpdateRoleFunctionResponse?>;

    public sealed record UpdateRoleFunctionResponse(
        Guid Id,
        Guid RoleId,
        Guid FunctionKeyId,
        string StatusCode);

    public sealed class UpdateRoleFunctionHandler
        : IRequestHandler<
            UpdateRoleFunctionCommand,
            UpdateRoleFunctionResponse?>
    {
        private readonly IRoleFunctionRepository _roleFunctionRepository;
        private readonly IRoleFunctionHistoryRepository _roleFunctionHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateRoleFunctionHandler(
            IRoleFunctionRepository roleFunctionRepository,
            IRoleFunctionHistoryRepository roleFunctionHistoryRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _roleFunctionRepository = roleFunctionRepository;
            _roleFunctionHistoryRepository = roleFunctionHistoryRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<UpdateRoleFunctionResponse?> Handle(
            UpdateRoleFunctionCommand command,
            CancellationToken cancellationToken)
        {
            var roleFunction = await _roleFunctionRepository.GetByIdAsync(
                command.RoleFunctionId,
                cancellationToken);

            if (roleFunction is null)
            {
                return null;
            }

            roleFunction.RoleId = command.RoleId;
            roleFunction.FunctionKeyId = command.FunctionKeyId;

            roleFunction.SetUpdated(
                _currentUserService.UserId,
                DateTimeOffset.UtcNow);

            var history = new RoleFunctionHistory
            {
                RoleFunctionId = roleFunction.Id,
                RoleId = roleFunction.RoleId,
                FunctionKeyId = roleFunction.FunctionKeyId,
                CreatedBy = roleFunction.CreatedBy,
                CreatedAt = roleFunction.CreatedAt,
                StatusCode = roleFunction.StatusCode,
                ActionTypeCode = ActionTypeCodes.Update,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            if (roleFunction.UpdatedBy.HasValue &&
                roleFunction.UpdatedAt.HasValue)
            {
                history.SetUpdated(
                    roleFunction.UpdatedBy.Value,
                    roleFunction.UpdatedAt.Value);
            }

            await _roleFunctionHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new UpdateRoleFunctionResponse(
                roleFunction.Id,
                roleFunction.RoleId,
                roleFunction.FunctionKeyId,
                roleFunction.StatusCode);
        }
    }
}