using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Positions
{
    public sealed record GetPositionByIdQuery(Guid PositionId)
        : IRequest<GetPositionByIdResponse?>;

    public sealed record GetPositionByIdResponse(
        Guid Id,
        string Name,
        Guid DepartmentId,
        string StatusCode);

    public sealed class GetPositionByIdHandler
        : IRequestHandler<GetPositionByIdQuery, GetPositionByIdResponse?>
    {
        private readonly IPositionRepository _positionRepository;

        public GetPositionByIdHandler(
            IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<GetPositionByIdResponse?> Handle(
            GetPositionByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            var position = await _positionRepository.GetByIdAsync(
                query.PositionId,
                cancellationToken);

            if (position is null)
            {
                return null;
            }

            return new GetPositionByIdResponse(
                position.Id,
                position.Name,
                position.DepartmentId,
                position.StatusCode);
        }
    }
}