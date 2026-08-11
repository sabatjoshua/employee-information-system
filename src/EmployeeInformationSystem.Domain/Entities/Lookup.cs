using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
