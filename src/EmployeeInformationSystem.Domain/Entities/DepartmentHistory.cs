using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class DepartmentHistory : HistoryEntity
    {
        public required Guid DepartmentId { get; set; }
        public required string Name { get; set; }
    }
}
