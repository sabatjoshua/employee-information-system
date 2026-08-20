using EmployeeInformationSystem.Application.Features.Employees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
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
                request.PositionId);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        [Authorize]
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
                request.PositionId);

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
            var command = new DeleteEmployeeCommand(
                id);

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
            var query = new GetEmployeeByIdQuery(id);

            var result = await _mediator.Send(
                query,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken)
        {
            var query = new GetEmployeesQuery();

            var result = await _mediator.Send(
                query,
                cancellationToken);

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
        Guid PositionId);

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
        Guid PositionId);
}