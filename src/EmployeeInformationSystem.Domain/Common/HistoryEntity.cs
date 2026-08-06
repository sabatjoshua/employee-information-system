using System;

namespace EmployeeInformationSystem.Domain.Common
{
    public abstract class HistoryEntity : AuditableEntity
    {
        public required string ActionTypeCode { get; set; }
        public required Guid ActionBy { get; set; }
        public required DateTimeOffset ActionAt { get; set; }
    }
}
