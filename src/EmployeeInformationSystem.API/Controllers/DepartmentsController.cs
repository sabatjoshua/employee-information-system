using EmployeeInformationSystem.Application.Features.Departments;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly CreateDepartmentHandler _createDepartmentHandler;

        public DepartmentsController(
            CreateDepartmentHandler createDepartmentHandler)
        {
            _createDepartmentHandler = createDepartmentHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateDepartmentRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateDepartmentCommand(
                request.Name,
                request.CreatedBy);

            var result = await _createDepartmentHandler.HandleAsync(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(Create),
                new { id = result.Id },
                result);
        }
    }
    public sealed record CreateDepartmentRequest(
        string Name,
        Guid CreatedBy);
}
