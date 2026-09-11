using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Files;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.EmployeeFiles
{
    public sealed record UploadEmployeeFileCommand(
        Guid EmployeeId,
        Stream FileStream,
        string OriginalFileName,
        string ContentType,
        long FileSize)
        : IRequest<UploadEmployeeFileResponse>;

    public sealed record UploadEmployeeFileResponse(
        Guid Id,
        Guid EmployeeId,
        string OriginalFileName,
        string StoredFileName,
        long FileSize);

    public sealed class UploadEmployeeFileHandler
        : IRequestHandler<UploadEmployeeFileCommand, UploadEmployeeFileResponse>
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IEmployeeFileRepository _employeeFileRepository;
        private readonly IEmployeeFileHistoryRepository _employeeFileHistoryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public UploadEmployeeFileHandler(
            IFileStorageService fileStorageService,
            IEmployeeFileRepository employeeFileRepository,
            IEmployeeFileHistoryRepository employeeFileHistoryRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _fileStorageService = fileStorageService;
            _employeeFileRepository = employeeFileRepository;
            _employeeFileHistoryRepository = employeeFileHistoryRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UploadEmployeeFileResponse> Handle(
            UploadEmployeeFileCommand command,
            CancellationToken cancellationToken)
        {
            var extension = Path.GetExtension(command.OriginalFileName);

            var storedFileName =
                $"{Guid.NewGuid():N}{extension}";

            var filePath = await _fileStorageService.SaveAsync(
                command.FileStream,
                storedFileName,
                cancellationToken);

            var employeeFile = new EmployeeFile
            {
                EmployeeId = command.EmployeeId,
                FilePath = filePath,
                OriginalFileName = command.OriginalFileName,
                StoredFileName = storedFileName,
                Extension = extension,
                FileSize = command.FileSize,
                ContentType = command.ContentType,
                StorageType = "LOCAL",
                StoragePath = filePath,
                StatusCode = StatusCodes.Active,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var history = new EmployeeFileHistory
            {
                EmployeeFileId = employeeFile.Id,
                EmployeeId = employeeFile.EmployeeId,
                FilePath = employeeFile.FilePath,
                OriginalFileName = employeeFile.OriginalFileName,
                StoredFileName = employeeFile.StoredFileName,
                Extension = employeeFile.Extension,
                FileSize = employeeFile.FileSize,
                ContentType = employeeFile.ContentType,
                StorageType = employeeFile.StorageType,
                StoragePath = employeeFile.StoragePath,
                StatusCode = employeeFile.StatusCode,
                CreatedBy = employeeFile.CreatedBy,
                CreatedAt = employeeFile.CreatedAt,
                ActionTypeCode = ActionTypeCodes.Insert,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            await _employeeFileRepository.AddAsync(
                employeeFile,
                cancellationToken);

            await _employeeFileHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new UploadEmployeeFileResponse(
                employeeFile.Id,
                employeeFile.EmployeeId,
                employeeFile.OriginalFileName,
                employeeFile.StoredFileName,
                employeeFile.FileSize);
        }
    }
}