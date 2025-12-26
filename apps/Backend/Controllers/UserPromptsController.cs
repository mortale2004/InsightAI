using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/user-prompts")]
  public class UserPromptsController : ControllerBase
  {
    private readonly UserPromptService _service;

    public UserPromptsController(UserPromptService service)
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
    public async Task<IActionResult> Create(UserPrompt model)
        => Ok(await _service.Create(model));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.Delete(id) ? Ok() : NotFound();
  }
}
