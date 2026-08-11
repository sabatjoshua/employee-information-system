using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class EmployeeHistory :HistoryEntity
    {
        public required Guid EmployeeId { get; set; }
        public required string EmployeeNo { get; set; }
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        public required string GenderCode { get; set; }
        public required DateTimeOffset BirthDate { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public required DateTimeOffset HireDate { get; set; }
        public required Guid DepartmentId { get; set; }
        public required Guid PositionId { get; set; }
    }
}
