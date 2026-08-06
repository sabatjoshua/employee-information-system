using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class FunctionKey : AuditableEntity
    {
        public required string FunctionCode { get; set; }
        public required string DisplayName { get; set; }
        public string? Remarks { get; set; }
    }
}