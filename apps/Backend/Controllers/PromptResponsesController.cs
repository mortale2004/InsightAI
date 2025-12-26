using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/prompt-responses")]
  public class PromptResponsesController : ControllerBase
  {
    private readonly PromptResponseService _service;

    public PromptResponsesController(PromptResponseService service)
    {
      _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetAll());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      var item = await _service.GetById(id);
      return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PromptResponse model)
        => Ok(await _service.Create(model));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.Delete(id) ? Ok() : NotFound();
  }
}
