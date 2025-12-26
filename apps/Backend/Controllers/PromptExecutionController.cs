using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
  [Authorize]
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
      var result = await _service.ExecuteAsync(
          request.PromptId,
          request.ResponseTypeId,
          request.ApplicationId,
          User
      );

      return Ok(new { response = result });
    }
  }

  public record ExecutePromptRequest(
      int PromptId,
      int ResponseTypeId,
      int ApplicationId
  );
}
