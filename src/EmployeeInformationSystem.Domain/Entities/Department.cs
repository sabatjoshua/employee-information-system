using EmployeeInformationSystem.Domain.Common;

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