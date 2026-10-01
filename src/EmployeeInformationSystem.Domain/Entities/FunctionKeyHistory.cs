using EmployeeInformationSystem.Domain.Common;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class FunctionKeyHistory : HistoryEntity
    {
        public required Guid FunctionKeyId { get; set; }
        public required string FunctionCode { get; set; }
        public required string DisplayName { get; set; }
        public string? Remarks { get; set; }
    }
}