using System;

namespace EmployeeInformationSystem.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();

        public required string StatusCode { get; set; }
    }
}
