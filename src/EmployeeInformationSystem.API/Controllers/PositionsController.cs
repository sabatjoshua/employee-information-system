using EmployeeInformationSystem.Application.Features.Positions;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionsController : ControllerBase
    {
        private readonly CreatePositionHandler _createPositionHandler;
        private readonly GetPositionByIdHandler _getPositionByIdHandler;
        private readonly UpdatePositionHandler _updatePositionHandler;
        private readonly DeletePositionHandler _deletePositionHandler;

        public PositionsController(
            CreatePositionHandler createPositionHandler,
            GetPositionByIdHandler getPositionByIdHandler,
            UpdatePositionHandler updatePositionHandler,
            DeletePositionHandler deletePositionHandler)
        {
            _createPositionHandler = createPositionHandler;
            _getPositionByIdHandler = getPositionByIdHandler;
            _updatePositionHandler = updatePositionHandler;
            _deletePositionHandler = deletePositionHandler;
        }


        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreatePositionRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreatePositionCommand(
                request.Name,
                request.DepartmentId,
                request.CreatedBy);

            var result = await _createPositionHandler.HandleAsync(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(Create),
                new { id = result.Id },
                result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdatePositionRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdatePositionCommand(
                id,
                request.Name,
                request.DepartmentId,
                request.UpdatedBy);

            var result = await _updatePositionHandler.HandleAsync(
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
            [FromBody] DeletePositionRequest request,
            CancellationToken cancellationToken)
        {
            var command = new DeletePositionCommand(
                id,
                request.DeletedBy);

            var result = await _deletePositionHandler.HandleAsync(
                command,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetPositionByIdQuery(id);

            var result = await _getPositionByIdHandler.HandleAsync(
                query,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
    public sealed record CreatePositionRequest(
        string Name,
        Guid DepartmentId,
        Guid CreatedBy);

    public sealed record UpdatePositionRequest(
        string Name,
        Guid DepartmentId,
        Guid UpdatedBy);

    public sealed record DeletePositionRequest(
        Guid DeletedBy);
}
