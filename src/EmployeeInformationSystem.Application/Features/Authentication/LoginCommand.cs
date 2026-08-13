using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Application.Features.Users;
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
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public LoginHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
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

            var validPassword = _passwordHasher.Verify(
                command.Password,
                user.PasswordHash);

            if (!validPassword)
            {
                user.FailedLoginAttempt++;
                if (user.FailedLoginAttempt >= 5)
                {
                    user.IsLocked = true;
                }

                await _unitOfWork.SaveChangesAsync(
                    cancellationToken);
                return null;
            }
            user.FailedLoginAttempt = 0;
            user.LastLogin = DateTimeOffset.UtcNow;

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new LoginResponse(
                user.Id,
                user.EmployeeId,
                user.UserName);
        }
    }
}
