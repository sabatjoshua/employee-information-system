using System;

namespace EmployeeInformationSystem.Domain.Common
{
    public abstract class AuditableEntity :BaseEntity
    {
        public required Guid CreatedBy { get; set; }

        public required DateTimeOffset CreatedAt { get; set; }

        public Guid? UpdatedBy { get; protected set; }

        public DateTimeOffset? UpdatedAt { get; protected set; }

    }
}
