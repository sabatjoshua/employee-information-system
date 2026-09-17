using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{

    public class RoleFunction : AuditableEntity
    {
        public RoleFunction()
        {
        }

        public RoleFunction(Guid id)
        {
            Id = id;
        }
        public required Guid RoleId { get; set; }
        public required Guid FunctionKeyId { get; set; }
    }
}