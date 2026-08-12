using EmployeeInformationSystem.Application.Features.Departments;
using EmployeeInformationSystem.Application.Features.Employees;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly CreateDepartmentHandler _createDepartmentHandler;
        private readonly GetDepartmentByIdHandler _getDepartmentByIdHandler;
        private readonly UpdateDepartmentHandler _updateDepartmentHandler;
        private readonly DeleteDepartmentHandler _deleteDepartmentHandler;

        public DepartmentsController(
            CreateDepartmentHandler createDepartmentHandler,
            GetDepartmentByIdHandler getDepartmentByIdHandler,
            UpdateDepartmentHandler updateDepartmentHandler,
            DeleteDepartmentHandler deleteDepartmentHandler)
        {
            _createDepartmentHandler = createDepartmentHandler;
            _getDepartmentByIdHandler = getDepartmentByIdHandler;
            _updateDepartmentHandler = updateDepartmentHandler;
            _deleteDepartmentHandler = deleteDepartmentHandler;
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

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateDepartmentRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateDepartmentCommand(
                id,
                request.Name,
                request.UpdatedBy);

            var result = await _updateDepartmentHandler.HandleAsync(
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
            [FromBody] DeleteDepartmentRequest request,
            CancellationToken cancellationToken)
        {
            var command = new DeleteDepartmentCommand(
                id,
                request.DeletedBy);

            var result = await _deleteDepartmentHandler.HandleAsync(
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
            var query = new GetDepartmentByIdQuery(id);

            var result = await _getDepartmentByIdHandler.HandleAsync(
                query,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
    public sealed record CreateDepartmentRequest(
        string Name,
        Guid CreatedBy);
    public sealed record UpdateDepartmentRequest(
    string Name,
    Guid UpdatedBy); public sealed record DeleteDepartmentRequest(
    Guid DeletedBy);
}
