using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.FunctionKeys
{
    public sealed record DeleteFunctionKeyCommand(
        Guid FunctionKeyId)
        : IRequest<DeleteFunctionKeyResponse?>;

    public sealed record DeleteFunctionKeyResponse(
        Guid Id,
        string FunctionCode,
        string DisplayName,
        string? Remarks,
        string StatusCode);

    public sealed class DeleteFunctionKeyHandler
        : IRequestHandler<DeleteFunctionKeyCommand, DeleteFunctionKeyResponse?>
    {
        private readonly IFunctionKeyRepository _functionKeyRepository;
        private readonly IFunctionKeyHistoryRepository _functionKeyHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteFunctionKeyHandler(
            IFunctionKeyRepository functionKeyRepository,
            IFunctionKeyHistoryRepository functionKeyHistoryRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _functionKeyRepository = functionKeyRepository;
            _functionKeyHistoryRepository = functionKeyHistoryRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<DeleteFunctionKeyResponse?> Handle(
            DeleteFunctionKeyCommand command,
            CancellationToken cancellationToken)
        {
            var functionKey = await _functionKeyRepository.GetByIdAsync(
                command.FunctionKeyId,
                cancellationToken);

            if (functionKey is null)
            {
                return null;
            }

            functionKey.StatusCode = StatusCodes.Inactive;

            var history = new FunctionKeyHistory
            {
                FunctionKeyId = functionKey.Id,
                FunctionCode = functionKey.FunctionCode,
                DisplayName = functionKey.DisplayName,
                Remarks = functionKey.Remarks,
                CreatedBy = functionKey.CreatedBy,
                CreatedAt = functionKey.CreatedAt,
                StatusCode = functionKey.StatusCode,
                ActionTypeCode = ActionTypeCodes.Delete,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            if (functionKey.UpdatedBy.HasValue &&
                functionKey.UpdatedAt.HasValue)
            {
                history.SetUpdated(
                    functionKey.UpdatedBy.Value,
                    functionKey.UpdatedAt.Value);
            }

            await _functionKeyHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new DeleteFunctionKeyResponse(
                functionKey.Id,
                functionKey.FunctionCode,
                functionKey.DisplayName,
                functionKey.Remarks,
                functionKey.StatusCode);
        }
    }
}