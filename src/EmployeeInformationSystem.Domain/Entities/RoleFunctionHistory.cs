using EmployeeInformationSystem.Domain.Common;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class RoleFunctionHistory : HistoryEntity
    {
        public required Guid RoleFunctionId { get; set; }
        public required Guid RoleId { get; set; }
        public required Guid FunctionKeyId { get; set; }
    }
}