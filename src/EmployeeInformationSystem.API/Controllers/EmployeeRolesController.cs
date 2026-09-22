using EmployeeInformationSystem.API.Authorization;
using EmployeeInformationSystem.Application.Common.Security;
using EmployeeInformationSystem.Application.Features.EmployeeRoles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeRolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeRolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HasPermission(Permissions.EmployeeRoleCreate)]
        [HttpPost]
        [ProducesResponseType(
            typeof(CreateEmployeeRoleResponse),
            StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create(
            [FromBody] CreateEmployeeRoleRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateEmployeeRoleCommand(
                request.EmployeeId,
                request.RoleId);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(Create),
                new { id = result.Id },
                result);
        }

        [HasPermission(Permissions.EmployeeRoleUpdate)]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(
            typeof(UpdateEmployeeRoleResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateEmployeeRoleRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateEmployeeRoleCommand(
                id,
                request.EmployeeId,
                request.RoleId);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HasPermission(Permissions.EmployeeRoleDelete)]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(
            typeof(DeleteEmployeeRoleResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteEmployeeRoleCommand(id);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HasPermission(Permissions.EmployeeRoleView)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(
            typeof(GetEmployeeRoleByIdResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetEmployeeRoleByIdQuery(id);

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

    public sealed record CreateEmployeeRoleRequest(
        Guid EmployeeId,
        Guid RoleId);

    public sealed record UpdateEmployeeRoleRequest(
        Guid EmployeeId,
        Guid RoleId);
}