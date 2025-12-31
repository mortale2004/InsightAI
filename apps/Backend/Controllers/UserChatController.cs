using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/user-chats")]
    public class UserChatController : ControllerBase
    {
        private readonly UserChatService _service;

        public UserChatController(UserChatService service)
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
        public async Task<IActionResult> Create([FromBody] UserChatRequest request)
        {
            return Ok(await _service.Create(request));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => await _service.Delete(id) ? Ok() : NotFound();
    }

    public class UserChatRequest : UserChatContextData
    {
        public int UserId { get; set; }
        public string ApplicationName { get; set; } = null!;
        public string RegionName { get; set; } = null!;
    }

}
