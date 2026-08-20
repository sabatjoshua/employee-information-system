using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Users
{
    public sealed record UpdateUserCommand(
        Guid UserId,
        Guid EmployeeId,
        string UserName,
        string? Password,
        bool MustChangePassword,
        bool IsLocked)
        : IRequest<UpdateUserResponse?>;

    public sealed record UpdateUserResponse(
        Guid Id,
        Guid EmployeeId,
        string UserName,
        bool MustChangePassword,
        bool IsLocked,
        string StatusCode);

    public sealed class UpdateUserHandler
        : IRequestHandler<UpdateUserCommand, UpdateUserResponse?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHistoryRepository _userHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserHandler(
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

        public async Task<UpdateUserResponse?> Handle(
            UpdateUserCommand command,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(
                command.UserId,
                cancellationToken);

            if (user is null)
            {
                return null;
            }

            user.EmployeeId = command.EmployeeId;
            user.UserName = command.UserName;
            user.MustChangePassword = command.MustChangePassword;
            user.IsLocked = command.IsLocked; 
            if (!string.IsNullOrWhiteSpace(command.Password))
            {
                user.PasswordHash = _passwordHasher.Hash(command.Password);
                user.PasswordChangedDate = DateTimeOffset.UtcNow;
            }

            user.SetUpdated(
                _currentUserService.UserId,
                DateTimeOffset.UtcNow);

            var history = new UserHistory
            {
                UserId = user.Id,
                EmployeeId = user.EmployeeId,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                LastLogin = user.LastLogin,
                FailedLoginAttempt = user.FailedLoginAttempt,
                PasswordChangedDate = user.PasswordChangedDate,
                MustChangePassword = user.MustChangePassword,
                IsLocked = user.IsLocked,
                StatusCode = user.StatusCode,
                CreatedBy = user.CreatedBy,
                CreatedAt = user.CreatedAt,
                ActionTypeCode = ActionTypeCodes.Update,
                ActionBy = _currentUserService.UserId,
                ActionAt = DateTimeOffset.UtcNow
            };

            if (user.UpdatedBy.HasValue &&
                user.UpdatedAt.HasValue)
            {
                history.SetUpdated(
                    user.UpdatedBy.Value,
                    user.UpdatedAt.Value);
            }

            await _userHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new UpdateUserResponse(
                user.Id,
                user.EmployeeId,
                user.UserName,
                user.MustChangePassword,
                user.IsLocked,
                user.StatusCode);
        }
    }
}