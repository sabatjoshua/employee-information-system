using EmployeeInformationSystem.Application.Features.FunctionKeys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost]
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

        [Authorize]
        [HttpPut("{id:guid}")]
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

        [Authorize]
        [HttpDelete("{id:guid}")]
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

        [Authorize]
        [HttpGet("{id:guid}")]
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