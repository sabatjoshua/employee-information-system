using EmployeeInformationSystem.API.Authorization;
using EmployeeInformationSystem.Application.Common.Security;
using EmployeeInformationSystem.Application.Features.Roles;
using MediatR;
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

        [HasPermission(Permissions.RoleCreate)]
        [HttpPost]
        [ProducesResponseType(
            typeof(CreateRoleResponse),
            StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        [HasPermission(Permissions.RoleUpdate)]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(
            typeof(UpdateRoleResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HasPermission(Permissions.RoleDelete)]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(
            typeof(DeleteRoleResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HasPermission(Permissions.RoleView)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(
            typeof(GetRoleByIdResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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