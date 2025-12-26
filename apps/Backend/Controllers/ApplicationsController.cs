using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/applications")]
  public class ApplicationsController : ControllerBase
  {
    private readonly ApplicationService _service;

    public ApplicationsController(ApplicationService service)
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
    public async Task<IActionResult> Create(Application model)
        => Ok(await _service.Create(model));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Application model)
        => await _service.Update(id, model) ? Ok() : NotFound();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.Delete(id) ? Ok() : NotFound();
  }
}
