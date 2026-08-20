using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Departments
{
    public sealed record CreateDepartmentCommand(string Name)
        : IRequest<CreateDepartmentResponse>;

    public sealed record CreateDepartmentResponse(
        Guid Id,
        string Name);

    public sealed class CreateDepartmentHandler
        : IRequestHandler<CreateDepartmentCommand, CreateDepartmentResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDepartmentHistoryRepository _departmentHistoryRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateDepartmentHandler(
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IDepartmentHistoryRepository departmentHistoryRepository,
            ICurrentUserService currentUserService)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _departmentHistoryRepository = departmentHistoryRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CreateDepartmentResponse> Handle(
            CreateDepartmentCommand command,
            CancellationToken cancellationToken)
        {
            var department = new Department
            {
                Name = command.Name,
                CreatedBy = _currentUserService.UserId,
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
                ActionBy = _currentUserService.UserId,
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
