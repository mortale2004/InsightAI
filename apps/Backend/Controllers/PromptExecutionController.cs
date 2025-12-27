using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/execute")]
    public class PromptExecutionController : ControllerBase
    {
        private readonly PromptExecutionService _service;

        public PromptExecutionController(PromptExecutionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Execute(ExecutePromptRequest request)
        {
            var result = await _service.ExecuteAsync(request);

            return Ok(new { response = result });
        }
    }

    public record ExecutePromptRequest(
        string ApplicationName,
        string RegionName,
        string FileName,
        string FileType,
        string FileContent,
        string? Prompt,
        ChildFileData[]? ChildFiles
    );

    public record ChildFileData(
        string FileName,
        string FileType,
        string FileContent
    );
}
