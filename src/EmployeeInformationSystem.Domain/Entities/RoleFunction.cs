using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{

    public class RoleFunction : AuditableEntity
    {
        public required Guid RoleId { get; set; }
        public required Guid FunctionKeyId { get; set; }
    }
}