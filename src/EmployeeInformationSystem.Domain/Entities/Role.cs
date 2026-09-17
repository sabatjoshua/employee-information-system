using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class Role : AuditableEntity
    {
        public Role()
        {
        }

        public Role(Guid id)
        {
            Id = id;
        }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}