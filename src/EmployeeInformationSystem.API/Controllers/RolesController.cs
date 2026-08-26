using EmployeeInformationSystem.Application.Features.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateRoleRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateRoleCommand(
                request.Name,
                request.Description);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(Create),
                new { id = result.Id },
                result);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateRoleRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateRoleCommand(
                id,
                request.Name,
                request.Description);

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
            var command = new DeleteRoleCommand(id);

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
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetRoleByIdQuery(id);

            var result = await _mediator.Send(
                query,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }

    public sealed record CreateRoleRequest(
        string Name,
        string? Description);

    public sealed record UpdateRoleRequest(
        string Name,
        string? Description);
}