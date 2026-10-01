
namespace EmployeeInformationSystem.Domain.Entities
{
    public class Lookup
    {
        public Guid LookupId { get; set; }

        public required string Type { get; set; }

        public required string Code { get; set; }

        public required string Name { get; set; }

        public required int Sort { get; set; }
    }
}
