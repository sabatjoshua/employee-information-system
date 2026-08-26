using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.FunctionKeys
{
    public sealed record UpdateFunctionKeyCommand(
        Guid FunctionKeyId,
        string FunctionCode,
        string DisplayName,
        string? Remarks)
        : IRequest<UpdateFunctionKeyResponse?>;

    public sealed record UpdateFunctionKeyResponse(
        Guid Id,
        string FunctionCode,
        string DisplayName,
        string? Remarks,
        string StatusCode);

    public sealed class UpdateFunctionKeyHandler
        : IRequestHandler<UpdateFunctionKeyCommand, UpdateFunctionKeyResponse?>
    {
        private readonly IFunctionKeyRepository _functionKeyRepository;
        private readonly IFunctionKeyHistoryRepository _functionKeyHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateFunctionKeyHandler(
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

        public async Task<UpdateFunctionKeyResponse?> Handle(
            UpdateFunctionKeyCommand command,
            CancellationToken cancellationToken)
        {
            var functionKey = await _functionKeyRepository.GetByIdAsync(
                command.FunctionKeyId,
                cancellationToken);

            if (functionKey is null)
            {
                return null;
            }

            functionKey.FunctionCode = command.FunctionCode;
            functionKey.DisplayName = command.DisplayName;
            functionKey.Remarks = command.Remarks;

            functionKey.SetUpdated(
                _currentUserService.UserId,
                DateTimeOffset.UtcNow);

            var history = new FunctionKeyHistory
            {
                FunctionKeyId = functionKey.Id,
                FunctionCode = functionKey.FunctionCode,
                DisplayName = functionKey.DisplayName,
                Remarks = functionKey.Remarks,
                CreatedBy = functionKey.CreatedBy,
                CreatedAt = functionKey.CreatedAt,
                StatusCode = functionKey.StatusCode,
                ActionTypeCode = ActionTypeCodes.Update,
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

            return new UpdateFunctionKeyResponse(
                functionKey.Id,
                functionKey.FunctionCode,
                functionKey.DisplayName,
                functionKey.Remarks,
                functionKey.StatusCode);
        }
    }
}