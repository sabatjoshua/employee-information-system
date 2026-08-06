using System;

namespace EmployeeInformationSystem.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        public required string StatusCode { get; set; }
    }
}
