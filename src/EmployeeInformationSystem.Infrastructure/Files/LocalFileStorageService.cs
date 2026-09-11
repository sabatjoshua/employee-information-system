using EmployeeInformationSystem.Application.Common.Interfaces.Files;

namespace EmployeeInformationSystem.Infrastructure.Files
{
    public sealed class LocalFileStorageService : IFileStorageService
    {
        private readonly string _rootPath;

        public LocalFileStorageService()
        {
            _rootPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Storage",
                "EmployeeFiles");
        }

        public async Task<string> SaveAsync(
            Stream fileStream,
            string fileName,
            CancellationToken cancellationToken = default)
        {
            Directory.CreateDirectory(_rootPath);

            var physicalPath = Path.Combine(
                _rootPath,
                fileName);

            await using var outputStream = new FileStream(
                physicalPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                81920,
                useAsync: true);

            await fileStream.CopyToAsync(
                outputStream,
                cancellationToken);

            // Return relative storage path, not physical Windows path
            return fileName;
        }

        public Task<Stream> GetAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            var physicalPath = Path.Combine(
                _rootPath,
                filePath);

            Stream stream = new FileStream(
                physicalPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

            return Task.FromResult(stream);
        }

        public Task DeleteAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            var physicalPath = Path.Combine(
                _rootPath,
                filePath);

            if (File.Exists(physicalPath))
                File.Delete(physicalPath);

            return Task.CompletedTask;
        }
    }
}