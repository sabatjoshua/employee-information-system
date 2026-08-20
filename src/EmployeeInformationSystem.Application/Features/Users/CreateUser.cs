using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Users
{
    public sealed record CreateUserCommand(
        Guid EmployeeId,
        string UserName,
        string Password)
        : IRequest<CreateUserResponse>;

    public sealed record CreateUserResponse(
        Guid Id,
        Guid EmployeeId,
        string UserName);

    public sealed class CreateUserHandler
        : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHistoryRepository _userHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICurrentUserService _currentUserService;

        public CreateUserHandler(
            IUserRepository userRepository,
            IUserHistoryRepository userHistoryRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _userHistoryRepository = userHistoryRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _currentUserService = currentUserService;
        }

        public async Task<CreateUserResponse> Handle(
            CreateUserCommand command,
            CancellationToken cancellationToken)
        {
            var user = new User
            {
                EmployeeId = command.EmployeeId,
                UserName = command.UserName,
                PasswordHash = _passwordHasher.Hash(command.Password),
                FailedLoginAttempt = 0,
                MustChangePassword = true,
                IsLocked = false,
                StatusCode = StatusCodes.Active,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var history = new UserHistory
            {
                UserId = user.Id,
                EmployeeId = user.EmployeeId,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                FailedLoginAttempt = user.FailedLoginAttempt,
                MustChangePassword = user.MustChangePassword,
                IsLocked = user.IsLocked,
                StatusCode = user.StatusCode,
                CreatedBy = user.CreatedBy,
                CreatedAt = user.CreatedAt,
                ActionTypeCode = ActionTypeCodes.Insert,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            await _userRepository.AddAsync(
                user,
                cancellationToken);

            await _userHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new CreateUserResponse(
                user.Id,
                user.EmployeeId,
                user.UserName);
        }
    }
}