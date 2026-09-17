using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class EmployeeRole : AuditableEntity
    {
        public EmployeeRole()
        {
        }

        public EmployeeRole(Guid id)
        {
            Id = id;
        }

        public required Guid EmployeeId { get; set; }
        public required Guid RoleId { get; set; }
    }
}