using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class PositionHistory : HistoryEntity
    {
        public required Guid PositionId { get; set; }
        public required string Name { get; set; }
        public required Guid DepartmentId { get; set; }
    }
}