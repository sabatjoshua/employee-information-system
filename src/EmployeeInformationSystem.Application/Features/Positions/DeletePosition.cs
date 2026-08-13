using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Positions
{
    public sealed record DeletePositionCommand(
        Guid PositionId,
        Guid DeletedBy)
        : IRequest<DeletePositionResponse?>;

    public sealed record DeletePositionResponse(
        Guid Id,
        string StatusCode);

    public sealed class DeletePositionHandler
        : IRequestHandler<DeletePositionCommand, DeletePositionResponse?>
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IPositionHistoryRepository _positionHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePositionHandler(
            IPositionRepository positionRepository,
            IPositionHistoryRepository positionHistoryRepository,
            IUnitOfWork unitOfWork)
        {
            _positionRepository = positionRepository;
            _positionHistoryRepository = positionHistoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeletePositionResponse?> Handle(
            DeletePositionCommand command,
            CancellationToken cancellationToken = default)
        {
            var position = await _positionRepository.GetByIdAsync(
                command.PositionId,
                cancellationToken);

            if (position is null)
            {
                return null;
            }

            position.StatusCode = StatusCodes.Inactive;

            var history = new PositionHistory
            {
                PositionId = position.Id,
                Name = position.Name,
                DepartmentId = position.DepartmentId,
                CreatedBy = position.CreatedBy,
                CreatedAt = position.CreatedAt,
                StatusCode = position.StatusCode,
                ActionTypeCode = ActionTypeCodes.Delete,
                ActionBy = command.DeletedBy,
                ActionAt = DateTimeOffset.UtcNow
            };

            if (position.UpdatedBy.HasValue && position.UpdatedAt.HasValue)
            {
                history.SetUpdated(
                    position.UpdatedBy.Value,
                    position.UpdatedAt.Value);
            }

            await _positionHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new DeletePositionResponse(
                position.Id,
                position.StatusCode);
        }
    }
}