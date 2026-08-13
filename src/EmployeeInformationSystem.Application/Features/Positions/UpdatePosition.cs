using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Positions
{
    public sealed record UpdatePositionCommand(
        Guid PositionId,
        string Name,
        Guid DepartmentId,
        Guid UpdatedBy)
        : IRequest<UpdatePositionResponse?>;

    public sealed record UpdatePositionResponse(
        Guid Id,
        string Name,
        Guid DepartmentId,
        string StatusCode);

    public sealed class UpdatePositionHandler
        : IRequestHandler<UpdatePositionCommand, UpdatePositionResponse?>
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IPositionHistoryRepository _positionHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePositionHandler(
            IPositionRepository positionRepository,
            IPositionHistoryRepository positionHistoryRepository,
            IUnitOfWork unitOfWork)
        {
            _positionRepository = positionRepository;
            _positionHistoryRepository = positionHistoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdatePositionResponse?> Handle(
            UpdatePositionCommand command,
            CancellationToken cancellationToken = default)
        {
            var position = await _positionRepository.GetByIdAsync(
                command.PositionId,
                cancellationToken);

            if (position is null)
            {
                return null;
            }

            position.Name = command.Name;
            position.DepartmentId = command.DepartmentId;
            position.SetUpdated(
                command.UpdatedBy,
                DateTimeOffset.UtcNow);

            var history = new PositionHistory
            {
                PositionId = position.Id,
                Name = position.Name,
                DepartmentId = position.DepartmentId,
                CreatedBy = position.CreatedBy,
                CreatedAt = position.CreatedAt,
                StatusCode = position.StatusCode,
                ActionTypeCode = ActionTypeCodes.Update,
                ActionBy = command.UpdatedBy,
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

            return new UpdatePositionResponse(
                position.Id,
                position.Name,
                position.DepartmentId,
                position.StatusCode);
        }
    }
}