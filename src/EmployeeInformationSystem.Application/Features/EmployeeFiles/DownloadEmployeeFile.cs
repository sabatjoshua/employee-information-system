using EmployeeInformationSystem.Application.Common.Interfaces.Files;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.EmployeeFiles
{
    public sealed record DownloadEmployeeFileQuery(
    Guid EmployeeId,
    Guid FileId)
    : IRequest<DownloadEmployeeFileResponse?>;

    public sealed record DownloadEmployeeFileResponse(
        Stream FileStream,
        string OriginalFileName,
        string ContentType);

    public sealed class DownloadEmployeeFileHandler
        : IRequestHandler<
            DownloadEmployeeFileQuery,
            DownloadEmployeeFileResponse?>
    {
        private readonly IEmployeeFileRepository _employeeFileRepository;
        private readonly IFileStorageService _fileStorageService;

        public DownloadEmployeeFileHandler(
            IEmployeeFileRepository employeeFileRepository,
            IFileStorageService fileStorageService)
        {
            _employeeFileRepository = employeeFileRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<DownloadEmployeeFileResponse?> Handle(
            DownloadEmployeeFileQuery request,
            CancellationToken cancellationToken)
        {
            var employeeFile = await _employeeFileRepository.GetByIdAsync(
                request.FileId,
                cancellationToken);

            if (employeeFile is null ||
                employeeFile.EmployeeId != request.EmployeeId)
            {
                return null;
            }

            var stream = await _fileStorageService.GetAsync(
                employeeFile.FilePath,
                cancellationToken);

            return new DownloadEmployeeFileResponse(
                stream,
                employeeFile.OriginalFileName,
                employeeFile.ContentType ?? "application/octet-stream");
        }
    }
}