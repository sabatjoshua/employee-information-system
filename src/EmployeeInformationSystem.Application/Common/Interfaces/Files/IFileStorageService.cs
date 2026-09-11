namespace EmployeeInformationSystem.Application.Common.Interfaces.Files
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(
            Stream fileStream,
            string fileName,
            CancellationToken cancellationToken = default);

        Task<Stream> GetAsync(
            string filePath,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            string filePath,
            CancellationToken cancellationToken = default);
    }
}