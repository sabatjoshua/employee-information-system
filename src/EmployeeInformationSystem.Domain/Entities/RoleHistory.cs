using EmployeeInformationSystem.Domain.Common;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class RoleHistory : HistoryEntity
    {
        public required Guid RoleId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}