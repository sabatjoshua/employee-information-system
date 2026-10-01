using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Positions
{
    public sealed record GetPositionsByDepartmentQuery(
        Guid DepartmentId)
        : IRequest<IReadOnlyList<GetPositionsByDepartmentResponse>>;

    public sealed record GetPositionsByDepartmentResponse(
        Guid Id,
        string Name);

    public sealed class GetPositionsByDepartmentHandler
        : IRequestHandler<
            GetPositionsByDepartmentQuery,
            IReadOnlyList<GetPositionsByDepartmentResponse>>
    {
        private readonly IPositionRepository _positionRepository;

        public GetPositionsByDepartmentHandler(
            IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<IReadOnlyList<GetPositionsByDepartmentResponse>> Handle(
            GetPositionsByDepartmentQuery request,
            CancellationToken cancellationToken)
        {
            var positions =
                await _positionRepository.GetByDepartmentIdAsync(
                    request.DepartmentId,
                    cancellationToken);

            return positions
                .Select(position => new GetPositionsByDepartmentResponse(
                    position.Id,
                    position.Name))
                .ToList();
        }
    }
}