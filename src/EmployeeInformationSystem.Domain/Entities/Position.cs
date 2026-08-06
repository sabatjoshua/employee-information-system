using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class Position : AuditableEntity
    {
        public required string Name { get; set; }
        public required Guid DepartmentId { get; set; }
    }
}