using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Application.Features.Positions
{
    public sealed record CreatePositionCommand(string Name, Guid DepartmentId, Guid CreatedBy);

    public sealed record CreatePositionResponse(
        Guid Id,
        string Name);

    public sealed class CreatePositionHandler
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPositionHistoryRepository _positionHistoryRepository;

        public CreatePositionHandler(
            IPositionRepository positionRepository,
            IUnitOfWork unitOfWork,
            IPositionHistoryRepository positionHistoryRepository)
        {
            _positionRepository = positionRepository;
            _unitOfWork = unitOfWork;
            _positionHistoryRepository = positionHistoryRepository;
        }

        public async Task<CreatePositionResponse> HandleAsync(
            CreatePositionCommand command,
            CancellationToken cancellationToken = default)
        {
            var position = new Position
            {
                Name = command.Name,
                DepartmentId = command.DepartmentId,
                CreatedBy = command.CreatedBy,
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
                ActionBy = command.CreatedBy,
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
