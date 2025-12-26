using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/auth")]
  public class AuthController : ControllerBase
  {
    private readonly AuthService _auth;

    public AuthController(AuthService auth)
    {
      _auth = auth;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
      var token = await _auth.Login(request.Email, request.Password);
      if (token == null)
        return Unauthorized("Invalid credentials");

      return Ok(new { token });
    }
  }

  public record LoginRequest(string Email, string Password);
}
