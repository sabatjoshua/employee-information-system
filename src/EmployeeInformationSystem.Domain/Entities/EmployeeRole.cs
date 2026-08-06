using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class EmployeeRole : AuditableEntity
    {
        public required Guid EmployeeId { get; set; }
        public required Guid RoleId { get; set; }
    }
}