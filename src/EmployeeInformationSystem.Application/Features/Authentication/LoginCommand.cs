using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Application.Features.Users;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Authentication
{
    public sealed record LoginCommand(
    string UserName,
    string Password)
    : IRequest<LoginResponse?>;
    public sealed record LoginResponse(
    Guid UserId,
    Guid EmployeeId,
    string UserName);

    public sealed class LoginHandler
        : IRequestHandler<LoginCommand, LoginResponse?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHistoryRepository _userHistoryRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public LoginHandler(
            IUserRepository userRepository,
            IUserHistoryRepository userHistoryRepository,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userHistoryRepository = userHistoryRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }
        public async Task<LoginResponse?> Handle(
            LoginCommand command,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserNameAsync(
                command.UserName,
                cancellationToken);

            if (user is null)
            {
                return null;
            }

            if (user.IsLocked)
            {
                return null;
            }

            var lastLogin = DateTimeOffset.UtcNow;

            var validPassword = _passwordHasher.Verify(
                command.Password,
                user.PasswordHash);
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
                ActionTypeCode = ActionTypeCodes.Login,
                ActionBy = user.EmployeeId,
                ActionAt = lastLogin
            };

            if (user.UpdatedBy.HasValue &&
                user.UpdatedAt.HasValue)
            {
                history.SetUpdated(
                    user.UpdatedBy.Value,
                    user.UpdatedAt.Value);
            }

            if (!validPassword)
            {
                user.FailedLoginAttempt++;
                if (user.FailedLoginAttempt >= 5)
                {
                    user.IsLocked = true;
                }

                history.FailedLoginAttempt = user.FailedLoginAttempt;
                history.IsLocked = user.IsLocked;

                await _userHistoryRepository.AddAsync(
                    history,
                    cancellationToken);

                await _unitOfWork.SaveChangesAsync(
                    cancellationToken);
                return null;
            }

            user.FailedLoginAttempt = 0;
            user.LastLogin = lastLogin;
            history.FailedLoginAttempt = user.FailedLoginAttempt;
            history.LastLogin = user.LastLogin;

            await _userHistoryRepository.AddAsync(
                history,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new LoginResponse(
                user.Id,
                user.EmployeeId,
                user.UserName);
        }
    }
}
