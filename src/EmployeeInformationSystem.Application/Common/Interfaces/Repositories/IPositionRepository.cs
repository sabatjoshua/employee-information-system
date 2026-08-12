using EmployeeInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Application.Common.Interfaces.Repositories
{
    public interface IPositionRepository
    {
        Task<Position?> GetByIdAsync(
            Guid positionId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Position position,
            CancellationToken cancellationToken = default);
    }
}
