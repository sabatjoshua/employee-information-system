using EmployeeInformationSystem.Application.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateUserCommand(
                request.EmployeeId,
                request.UserName,
                request.Password,
                request.CreatedBy);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery(id);

            var result = await _mediator.Send(
                query,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateUserCommand(
                id,
                request.EmployeeId,
                request.UserName,
                request.Password,
                request.MustChangePassword,
                request.IsLocked,
                request.UpdatedBy);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id,
            [FromBody] DeleteUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand(
                id,
                request.DeletedBy);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }

    public sealed record CreateUserRequest(
        Guid EmployeeId,
        string UserName,
        string Password,
        Guid CreatedBy);

    public sealed record UpdateUserRequest(
        Guid EmployeeId,
        string UserName,
        string? Password,
        bool MustChangePassword,
        bool IsLocked,
        Guid UpdatedBy);

    public sealed record DeleteUserRequest(
        Guid DeletedBy);
}