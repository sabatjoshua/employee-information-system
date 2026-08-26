using EmployeeInformationSystem.Application.Features.EmployeeRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost]
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

        [Authorize]
        [HttpPut("{id:guid}")]
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

        [Authorize]
        [HttpDelete("{id:guid}")]
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

        [Authorize]
        [HttpGet("{id:guid}")]
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