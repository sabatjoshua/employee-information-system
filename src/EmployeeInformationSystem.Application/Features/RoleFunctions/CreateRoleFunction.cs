using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.RoleFunctions
{
    public sealed record CreateRoleFunctionCommand(
        Guid RoleId,
        Guid FunctionKeyId)
        : IRequest<CreateRoleFunctionResponse>;

    public sealed record CreateRoleFunctionResponse(
        Guid Id,
        Guid RoleId,
        Guid FunctionKeyId);

    public sealed class CreateRoleFunctionHandler
        : IRequestHandler<CreateRoleFunctionCommand, CreateRoleFunctionResponse>
    {
        private readonly IRoleFunctionRepository _roleFunctionRepository;
        private readonly IRoleFunctionHistoryRepository _roleFunctionHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateRoleFunctionHandler(
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

        public async Task<CreateRoleFunctionResponse> Handle(
            CreateRoleFunctionCommand command,
            CancellationToken cancellationToken)
        {
            var roleFunction = new RoleFunction
            {
                RoleId = command.RoleId,
                FunctionKeyId = command.FunctionKeyId,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTimeOffset.UtcNow,
                StatusCode = StatusCodes.Active
            };

            var history = new RoleFunctionHistory
            {
                RoleFunctionId = roleFunction.Id,
                RoleId = roleFunction.RoleId,
                FunctionKeyId = roleFunction.FunctionKeyId,
                CreatedBy = roleFunction.CreatedBy,
                CreatedAt = roleFunction.CreatedAt,
                StatusCode = roleFunction.StatusCode,
                ActionTypeCode = ActionTypeCodes.Insert,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            await _roleFunctionRepository.AddAsync(
                roleFunction,
                cancellationToken);

            await _roleFunctionHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new CreateRoleFunctionResponse(
                roleFunction.Id,
                roleFunction.RoleId,
                roleFunction.FunctionKeyId);
        }
    }
}