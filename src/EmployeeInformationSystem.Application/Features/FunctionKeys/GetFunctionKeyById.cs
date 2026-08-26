using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.FunctionKeys
{
    public sealed record GetFunctionKeyByIdQuery(Guid FunctionKeyId)
        : IRequest<GetFunctionKeyByIdResponse?>;

    public sealed record GetFunctionKeyByIdResponse(
        Guid Id,
        string FunctionCode,
        string DisplayName,
        string? Remarks,
        string StatusCode);

    public sealed class GetFunctionKeyByIdHandler
        : IRequestHandler<GetFunctionKeyByIdQuery, GetFunctionKeyByIdResponse?>
    {
        private readonly IFunctionKeyRepository _functionKeyRepository;

        public GetFunctionKeyByIdHandler(
            IFunctionKeyRepository functionKeyRepository)
        {
            _functionKeyRepository = functionKeyRepository;
        }

        public async Task<GetFunctionKeyByIdResponse?> Handle(
            GetFunctionKeyByIdQuery request,
            CancellationToken cancellationToken)
        {
            var functionKey = await _functionKeyRepository.GetByIdAsync(
                request.FunctionKeyId,
                cancellationToken);

            if (functionKey is null)
            {
                return null;
            }

            return new GetFunctionKeyByIdResponse(
                functionKey.Id,
                functionKey.FunctionCode,
                functionKey.DisplayName,
                functionKey.Remarks,
                functionKey.StatusCode);
        }
    }
}