using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class EmployeeFilesHistory : HistoryEntity
    {
        public required Guid EmployeeFileId { get; set; }
        public required Guid EmployeeId { get; set; }
        public required string FilePath { get; set; }
        public required string OriginalFileName { get; set; }
        public required string StoredFileName { get; set; }
        public required string Extension { get; set; }
        public required long FileSize { get; set; }
        public string? ContentType { get; set; }
        public required string StorageType { get; set; }
        public string? StoragePath { get; set; }
    }
}