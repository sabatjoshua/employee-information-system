using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class Position : AuditableEntity
    {
        public Position()
        {
        }

        public Position(Guid id)
        {
            Id = id;
        }
        public required string Name { get; set; }
        public required Guid DepartmentId { get; set; }
    }
}