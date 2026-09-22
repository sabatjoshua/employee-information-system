using EmployeeInformationSystem.API.Authorization;
using EmployeeInformationSystem.Application.Common.Security;
using EmployeeInformationSystem.Application.Features.FunctionKeys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FunctionKeysController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FunctionKeysController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HasPermission(Permissions.FunctionCreate)]
        [HttpPost]
        [ProducesResponseType(
            typeof(CreateFunctionKeyResponse),
            StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create(
            [FromBody] CreateFunctionKeyRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateFunctionKeyCommand(
                request.FunctionCode,
                request.DisplayName,
                request.Remarks);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(Create),
                new { id = result.Id },
                result);
        }

        [HasPermission(Permissions.FunctionUpdate)]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(
            typeof(UpdateFunctionKeyResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateFunctionKeyRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateFunctionKeyCommand(
                id,
                request.FunctionCode,
                request.DisplayName,
                request.Remarks);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HasPermission(Permissions.FunctionDelete)]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(
            typeof(DeleteFunctionKeyResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteFunctionKeyCommand(id);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HasPermission(Permissions.FunctionView)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(
            typeof(GetFunctionKeyByIdResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetFunctionKeyByIdQuery(id);

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

    public sealed record CreateFunctionKeyRequest(
        string FunctionCode,
        string DisplayName,
        string? Remarks);

    public sealed record UpdateFunctionKeyRequest(
        string FunctionCode,
        string DisplayName,
        string? Remarks);
}