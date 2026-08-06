using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class EmployeeRoleHistory : HistoryEntity
    {
        public required Guid EmployeeRoleId { get; set; }
        public required Guid EmployeeId { get; set; }
        public required Guid RoleId { get; set; }
    }
}