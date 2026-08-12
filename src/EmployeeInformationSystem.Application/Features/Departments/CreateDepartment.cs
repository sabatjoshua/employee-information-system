using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Application.Features.Departments
{
    public sealed record CreateDepartmentCommand(string Name, Guid CreatedBy);

    public sealed record CreateDepartmentResponse(
        Guid Id,
        string Name);

    public sealed class CreateDepartmentHandler
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDepartmentHistoryRepository _departmentHistoryRepository;

        public CreateDepartmentHandler(
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IDepartmentHistoryRepository departmentHistoryRepository)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _departmentHistoryRepository = departmentHistoryRepository;
        }

        public async Task<CreateDepartmentResponse> HandleAsync(
            CreateDepartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            var department = new Department
            {
                Name = command.Name,
                CreatedBy = command.CreatedBy,
                CreatedAt = DateTimeOffset.UtcNow,
                StatusCode = StatusCodes.Active
            };

            var history = new DepartmentHistory
            {
                DepartmentId = department.Id,
                Name = department.Name,
                CreatedBy = department.CreatedBy,
                CreatedAt = department.CreatedAt,
                StatusCode = department.StatusCode,
                ActionTypeCode = ActionTypeCodes.Insert,
                ActionBy = command.CreatedBy,
                ActionAt = DateTimeOffset.UtcNow
            };

            await _departmentRepository.AddAsync(
                department,
                cancellationToken);

            await _departmentHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new CreateDepartmentResponse(
                department.Id,
                department.Name);
        }
    }
}
