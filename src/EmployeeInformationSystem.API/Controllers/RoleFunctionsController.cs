using EmployeeInformationSystem.Application.Features.RoleFunctions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost]
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

        [Authorize]
        [HttpPut("{id:guid}")]
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

        [Authorize]
        [HttpDelete("{id:guid}")]
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

        [Authorize]
        [HttpGet("{id:guid}")]
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