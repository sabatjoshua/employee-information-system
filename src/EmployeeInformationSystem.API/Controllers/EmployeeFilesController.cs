using EmployeeInformationSystem.API.Models.EmployeeFiles;
using EmployeeInformationSystem.Application.Features.EmployeeFiles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/employees/{employeeId:guid}/files")]
    [Authorize]
    public sealed class EmployeeFilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeFilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(
        Guid employeeId,
        [FromForm] UploadEmployeeFileRequest request,
        CancellationToken cancellationToken)
        {
            if (request.File is null || request.File.Length == 0)
                return BadRequest("File is empty.");

            await using var stream = request.File.OpenReadStream();

            var command = new UploadEmployeeFileCommand(
                employeeId,
                stream,
                request.File.FileName,
                request.File.ContentType,
                request.File.Length);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByEmployeeId(
            Guid employeeId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetEmployeeFilesQuery(employeeId),
                cancellationToken);

            return Ok(result);
        }
        [HttpGet("{fileId:guid}/download")]
        public async Task<IActionResult> Download(
            Guid employeeId,
            Guid fileId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new DownloadEmployeeFileQuery(
                    employeeId,
                    fileId),
                cancellationToken);

            if (result is null)
                return NotFound();

            return File(
                result.FileStream,
                result.ContentType,
                result.OriginalFileName);
        }
    }
}