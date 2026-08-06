using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class Department : AuditableEntity
    {
        public required string Name { get; set; }
    }
}
