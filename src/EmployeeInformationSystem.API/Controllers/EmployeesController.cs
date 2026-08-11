using EmployeeInformationSystem.Application.Features.Employees;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly GetEmployeeByIdHandler _handler;

        public EmployeesController(GetEmployeeByIdHandler handler)
        {
            _handler = handler;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetEmployeeByIdQuery(id);

            var result = await _handler.HandleAsync(
                query,
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
