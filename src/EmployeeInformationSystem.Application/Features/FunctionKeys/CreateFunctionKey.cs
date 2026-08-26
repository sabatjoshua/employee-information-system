using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.FunctionKeys
{
    public sealed record CreateFunctionKeyCommand(
        string FunctionCode,
        string DisplayName,
        string? Remarks)
        : IRequest<CreateFunctionKeyResponse>;

    public sealed record CreateFunctionKeyResponse(
        Guid Id,
        string FunctionCode,
        string DisplayName,
        string? Remarks);

    public sealed class CreateFunctionKeyHandler
        : IRequestHandler<CreateFunctionKeyCommand, CreateFunctionKeyResponse>
    {
        private readonly IFunctionKeyRepository _functionKeyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFunctionKeyHistoryRepository _functionKeyHistoryRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateFunctionKeyHandler(
            IFunctionKeyRepository functionKeyRepository,
            IUnitOfWork unitOfWork,
            IFunctionKeyHistoryRepository functionKeyHistoryRepository,
            ICurrentUserService currentUserService)
        {
            _functionKeyRepository = functionKeyRepository;
            _unitOfWork = unitOfWork;
            _functionKeyHistoryRepository = functionKeyHistoryRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CreateFunctionKeyResponse> Handle(
            CreateFunctionKeyCommand command,
            CancellationToken cancellationToken)
        {
            var functionKey = new FunctionKey
            {
                FunctionCode = command.FunctionCode,
                DisplayName = command.DisplayName,
                Remarks = command.Remarks,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTimeOffset.UtcNow,
                StatusCode = StatusCodes.Active
            };

            var history = new FunctionKeyHistory
            {
                FunctionKeyId = functionKey.Id,
                FunctionCode = functionKey.FunctionCode,
                DisplayName = functionKey.DisplayName,
                Remarks = functionKey.Remarks,
                CreatedBy = functionKey.CreatedBy,
                CreatedAt = functionKey.CreatedAt,
                StatusCode = functionKey.StatusCode,
                ActionTypeCode = ActionTypeCodes.Insert,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            await _functionKeyRepository.AddAsync(
                functionKey,
                cancellationToken);

            await _functionKeyHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new CreateFunctionKeyResponse(
                functionKey.Id,
                functionKey.FunctionCode,
                functionKey.DisplayName,
                functionKey.Remarks);
        }
    }
}