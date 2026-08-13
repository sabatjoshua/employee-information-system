using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Users
{
    public sealed record DeleteUserCommand(
        Guid UserId,
        Guid DeletedBy)
        : IRequest<DeleteUserResponse?>;

    public sealed record DeleteUserResponse(
        Guid Id,
        string StatusCode);

    public sealed class DeleteUserHandler
        : IRequestHandler<DeleteUserCommand, DeleteUserResponse?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHistoryRepository _userHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserHandler(
            IUserRepository userRepository,
            IUserHistoryRepository userHistoryRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userHistoryRepository = userHistoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteUserResponse?> Handle(
            DeleteUserCommand command,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(
                command.UserId,
                cancellationToken);

            if (user is null)
            {
                return null;
            }

            user.StatusCode = StatusCodes.Inactive;

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
                ActionTypeCode = ActionTypeCodes.Delete,
                ActionBy = command.DeletedBy,
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

            return new DeleteUserResponse(
                user.Id,
                user.StatusCode);
        }
    }
}