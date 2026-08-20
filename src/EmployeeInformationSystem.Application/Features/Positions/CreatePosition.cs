using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Positions
{
    public sealed record CreatePositionCommand(string Name, Guid DepartmentId)
        : IRequest<CreatePositionResponse>;

    public sealed record CreatePositionResponse(
        Guid Id,
        string Name);

    public sealed class CreatePositionHandler
        : IRequestHandler<CreatePositionCommand, CreatePositionResponse>
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPositionHistoryRepository _positionHistoryRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreatePositionHandler(
            IPositionRepository positionRepository,
            IUnitOfWork unitOfWork,
            IPositionHistoryRepository positionHistoryRepository,
            ICurrentUserService currentUserService)
        {
            _positionRepository = positionRepository;
            _unitOfWork = unitOfWork;
            _positionHistoryRepository = positionHistoryRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CreatePositionResponse> Handle(
            CreatePositionCommand command,
            CancellationToken cancellationToken = default)
        {
            var position = new Position
            {
                Name = command.Name,
                DepartmentId = command.DepartmentId,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTimeOffset.UtcNow,
                StatusCode = StatusCodes.Active
            };

            var history = new PositionHistory
            {
                PositionId = position.Id,
                Name = position.Name,
                DepartmentId= position.DepartmentId,
                CreatedBy = position.CreatedBy,
                CreatedAt = position.CreatedAt,
                StatusCode = position.StatusCode,
                ActionTypeCode = ActionTypeCodes.Insert,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            await _positionRepository.AddAsync(
                position,
                cancellationToken);

            await _positionHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new CreatePositionResponse(
                position.Id,
                position.Name);
        }
    }
}
