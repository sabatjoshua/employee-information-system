using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.EmployeeFiles
{
    public sealed record GetEmployeeFilesQuery(
        Guid EmployeeId)
        : IRequest<List<GetEmployeeFilesResponse>>;

    public sealed record GetEmployeeFilesResponse(
        Guid Id,
        Guid EmployeeId,
        string OriginalFileName,
        string StoredFileName,
        string Extension,
        long FileSize,
        string? ContentType,
        string StorageType,
        DateTimeOffset CreatedAt);

    public sealed class GetEmployeeFilesHandler
        : IRequestHandler<
            GetEmployeeFilesQuery,
            List<GetEmployeeFilesResponse>>
    {
        private readonly IEmployeeFileRepository _employeeFileRepository;

        public GetEmployeeFilesHandler(
            IEmployeeFileRepository employeeFileRepository)
        {
            _employeeFileRepository = employeeFileRepository;
        }

        public async Task<List<GetEmployeeFilesResponse>> Handle(
            GetEmployeeFilesQuery request,
            CancellationToken cancellationToken)
        {
            var files =
                await _employeeFileRepository.GetByEmployeeIdAsync(
                    request.EmployeeId,
                    cancellationToken);

            return files
                .Select(x => new GetEmployeeFilesResponse(
                    x.Id,
                    x.EmployeeId,
                    x.OriginalFileName,
                    x.StoredFileName,
                    x.Extension,
                    x.FileSize,
                    x.ContentType,
                    x.StorageType,
                    x.CreatedAt))
                .ToList();
        }
    }
}