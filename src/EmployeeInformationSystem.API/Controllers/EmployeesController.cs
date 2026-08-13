using EmployeeInformationSystem.Application.Features.Employees;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly CreateEmployeeHandler _createEmployeeHandler;
        private readonly GetEmployeeByIdHandler _getEmployeeByIdHandler;
        private readonly UpdateEmployeeHandler _updateEmployeeHandler;
        private readonly DeleteEmployeeHandler _deleteEmployeeHandler;

        public EmployeesController(
            CreateEmployeeHandler createEmployeeHandler,
            GetEmployeeByIdHandler getEmployeeByIdHandler,
            UpdateEmployeeHandler updateEmployeeHandler,
            DeleteEmployeeHandler deleteEmployeeHandler)
        {
            _createEmployeeHandler = createEmployeeHandler;
            _getEmployeeByIdHandler = getEmployeeByIdHandler;
            _updateEmployeeHandler = updateEmployeeHandler;
            _deleteEmployeeHandler = deleteEmployeeHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateEmployeeRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateEmployeeCommand(
                request.EmployeeNo,
                request.FirstName,
                request.MiddleName,
                request.LastName,
                request.GenderCode,
                request.BirthDate,
                request.Email,
                request.MobileNo,
                request.HireDate,
                request.DepartmentId,
                request.PositionId,
                request.CreatedBy);

            var result = await _createEmployeeHandler.HandleAsync(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateEmployeeRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateEmployeeCommand(
                id,
                request.EmployeeNo,
                request.FirstName,
                request.MiddleName,
                request.LastName,
                request.GenderCode,
                request.BirthDate,
                request.Email,
                request.MobileNo,
                request.HireDate,
                request.DepartmentId,
                request.PositionId,
                request.UpdatedBy);

            var result = await _updateEmployeeHandler.HandleAsync(
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
            [FromBody] DeleteEmployeeRequest request,
            CancellationToken cancellationToken)
        {
            var command = new DeleteEmployeeCommand(
                id,
                request.DeletedBy);

            var result = await _deleteEmployeeHandler.HandleAsync(
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
            var query = new GetEmployeeByIdQuery(id);

            var result = await _getEmployeeByIdHandler.HandleAsync(
                query,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }

    public sealed record CreateEmployeeRequest(
        string EmployeeNo,
        string FirstName,
        string? MiddleName,
        string LastName,
        string GenderCode,
        DateTimeOffset BirthDate,
        string? Email,
        string? MobileNo,
        DateTimeOffset HireDate,
        Guid DepartmentId,
        Guid PositionId,
        Guid CreatedBy);

    public sealed record UpdateEmployeeRequest(
        string EmployeeNo,
        string FirstName,
        string? MiddleName,
        string LastName,
        string GenderCode,
        DateTimeOffset BirthDate,
        string? Email,
        string? MobileNo,
        DateTimeOffset HireDate,
        Guid DepartmentId,
        Guid PositionId,
        Guid UpdatedBy);

    public sealed record DeleteEmployeeRequest(
        Guid DeletedBy);
}