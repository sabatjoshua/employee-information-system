using EmployeeInformationSystem.Application.Features.Positions;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionsController : ControllerBase
    {
        private readonly CreatePositionHandler _createPositionHandler;

        public PositionsController(
            CreatePositionHandler createPositionHandler)
        {
            _createPositionHandler = createPositionHandler;
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
    }
    public sealed record CreatePositionRequest(
        string Name,
        Guid DepartmentId,
        Guid CreatedBy);
}
