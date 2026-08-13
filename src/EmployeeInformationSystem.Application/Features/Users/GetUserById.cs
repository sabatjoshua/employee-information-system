using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using MediatR;

namespace EmployeeInformationSystem.Application.Features.Users
{
    public sealed record GetUserByIdQuery(Guid UserId)
        : IRequest<GetUserByIdResponse?>;

    public sealed record GetUserByIdResponse(
        Guid Id,
        Guid EmployeeId,
        string UserName,
        string PasswordHash,
        DateTimeOffset? LastLogin,
        int FailedLoginAttempt,
        DateTimeOffset? PasswordChangedDate,
        bool MustChangePassword,
        bool IsLocked,
        string StatusCode);

    public sealed class GetUserByIdHandler
        : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdHandler(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserByIdResponse?> Handle(
            GetUserByIdQuery query,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(
                query.UserId,
                cancellationToken);

            if (user is null)
            {
                return null;
            }

            return new GetUserByIdResponse(
                user.Id,
                user.EmployeeId,
                user.UserName,
                user.PasswordHash,
                user.LastLogin,
                user.FailedLoginAttempt,
                user.PasswordChangedDate,
                user.MustChangePassword,
                user.IsLocked,
                user.StatusCode);
        }
    }
}