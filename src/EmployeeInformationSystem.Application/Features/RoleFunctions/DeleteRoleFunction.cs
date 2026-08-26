using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.RoleFunctions
{
    public sealed record DeleteRoleFunctionCommand(
        Guid RoleFunctionId)
        : IRequest<DeleteRoleFunctionResponse?>;

    public sealed record DeleteRoleFunctionResponse(
        Guid Id,
        Guid RoleId,
        Guid FunctionKeyId,
        string StatusCode);

    public sealed class DeleteRoleFunctionHandler
        : IRequestHandler<
            DeleteRoleFunctionCommand,
            DeleteRoleFunctionResponse?>
    {
        private readonly IRoleFunctionRepository _roleFunctionRepository;
        private readonly IRoleFunctionHistoryRepository _roleFunctionHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteRoleFunctionHandler(
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

        public async Task<DeleteRoleFunctionResponse?> Handle(
            DeleteRoleFunctionCommand command,
            CancellationToken cancellationToken)
        {
            var roleFunction = await _roleFunctionRepository.GetByIdAsync(
                command.RoleFunctionId,
                cancellationToken);

            if (roleFunction is null)
            {
                return null;
            }

            roleFunction.StatusCode = StatusCodes.Inactive;

            var history = new RoleFunctionHistory
            {
                RoleFunctionId = roleFunction.Id,
                RoleId = roleFunction.RoleId,
                FunctionKeyId = roleFunction.FunctionKeyId,
                CreatedBy = roleFunction.CreatedBy,
                CreatedAt = roleFunction.CreatedAt,
                StatusCode = roleFunction.StatusCode,
                ActionTypeCode = ActionTypeCodes.Delete,
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

            return new DeleteRoleFunctionResponse(
                roleFunction.Id,
                roleFunction.RoleId,
                roleFunction.FunctionKeyId,
                roleFunction.StatusCode);
        }
    }
}