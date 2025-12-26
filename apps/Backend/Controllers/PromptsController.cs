using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/prompts")]
  public class PromptsController : ControllerBase
  {
    private readonly PromptService _service;

    public PromptsController(PromptService service)
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
    public async Task<IActionResult> Create(Prompt model)
        => Ok(await _service.Create(model));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Prompt model)
        => await _service.Update(id, model) ? Ok() : NotFound();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.Delete(id) ? Ok() : NotFound();
  }
}
