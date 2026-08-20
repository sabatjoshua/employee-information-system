using EmployeeInformationSystem.Application.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateUserCommand(
                request.EmployeeId,
                request.UserName,
                request.Password);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        [Authorize]
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

        [Authorize]
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
                request.IsLocked);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand(
                id);

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
        string Password);

    public sealed record UpdateUserRequest(
        Guid EmployeeId,
        string UserName,
        string? Password,
        bool MustChangePassword,
        bool IsLocked);
}