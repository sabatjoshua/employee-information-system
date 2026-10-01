using EmployeeInformationSystem.Domain.Common;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class Department : AuditableEntity
    {
        public Department()
        {
        }

        public Department(Guid id)
        {
            Id = id;
        }

        public required string Name { get; set; }
    }
}