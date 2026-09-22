using EmployeeInformationSystem.API.Authorization;
using EmployeeInformationSystem.API.Models.EmployeeFiles;
using EmployeeInformationSystem.Application.Common.Security;
using EmployeeInformationSystem.Application.Features.EmployeeFiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/employees/{employeeId:guid}/files")]
    public sealed class EmployeeFilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeFilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HasPermission(Permissions.EmployeeFileCreate)]
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(
            typeof(UploadEmployeeFileResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Upload(
            Guid employeeId,
            [FromForm] UploadEmployeeFileRequest request,
            CancellationToken cancellationToken)
        {
            if (request.File is null || request.File.Length == 0)
            {
                return BadRequest("File is empty.");
            }

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

        [HasPermission(Permissions.EmployeeFileView)]
        [HttpGet]
        [ProducesResponseType(
            typeof(GetEmployeeFilesResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByEmployeeId(
            Guid employeeId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetEmployeeFilesQuery(employeeId),
                cancellationToken);

            return Ok(result);
        }

        [HasPermission(Permissions.EmployeeFileView)]
        [HttpGet("{fileId:guid}/download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            {
                return NotFound();
            }

            return File(
                result.FileStream,
                result.ContentType,
                result.OriginalFileName);
        }
    }
}