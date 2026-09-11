namespace EmployeeInformationSystem.API.Models.EmployeeFiles
{
    public sealed class UploadEmployeeFileRequest
    {
        public required IFormFile File { get; set; }
    }
}
