namespace EmployeeInformationSystem.Domain.Common
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        protected BaseEntity(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; protected set; }

        public required string StatusCode { get; set; }
    }
}