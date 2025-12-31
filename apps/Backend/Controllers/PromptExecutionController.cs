using Azure.Core;
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
        public async Task Execute(ExecutePromptRequest request, CancellationToken cancellationToken)
        {
            Response.ContentType = "text/plain";
            Response.Headers.Append("Cache-Control", "no-cache");
            Response.Headers.Append("Connection", "keep-alive");

            await foreach (var chunk in _service.ExecutePromptStreamingAsync(request)
                                                .WithCancellation(cancellationToken))
            {
                await Response.WriteAsync(chunk, cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }
        }
    }

    public record ExecutePromptRequest(string Prompt, int UserChatId);
}
