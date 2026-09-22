using EmployeeInformationSystem.API.Authorization;
using EmployeeInformationSystem.Application.Common.Security;
using EmployeeInformationSystem.Application.Features.RoleFunctions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleFunctionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleFunctionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HasPermission(Permissions.RoleFunctionCreate)]
        [HttpPost]
        [ProducesResponseType(
            typeof(CreateRoleFunctionResponse),
            StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create(
            [FromBody] CreateRoleFunctionRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateRoleFunctionCommand(
                request.RoleId,
                request.FunctionKeyId);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(Create),
                new { id = result.Id },
                result);
        }

        [HasPermission(Permissions.RoleFunctionUpdate)]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(
            typeof(UpdateRoleFunctionResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateRoleFunctionRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateRoleFunctionCommand(
                id,
                request.RoleId,
                request.FunctionKeyId);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HasPermission(Permissions.RoleFunctionDelete)]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(
            typeof(DeleteRoleFunctionResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteRoleFunctionCommand(id);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HasPermission(Permissions.RoleFunctionView)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(
            typeof(GetRoleFunctionByIdResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetRoleFunctionByIdQuery(id);

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

    public sealed record CreateRoleFunctionRequest(
        Guid RoleId,
        Guid FunctionKeyId);

    public sealed record UpdateRoleFunctionRequest(
        Guid RoleId,
        Guid FunctionKeyId);
}