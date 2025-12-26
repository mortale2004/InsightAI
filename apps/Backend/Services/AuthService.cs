using Backend.Data;
using Backend.Helpers;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Services
{
  public class AuthService
  {
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IConfiguration config)
    {
      _db = db;
      _config = config;
    }

    public async Task<string?> Login(string email, string password)
    {
      var user = await _db.Users
          .FirstOrDefaultAsync(x => x.Email == email);

      if (user == null)
        return null;

      if (!PasswordHasher.Verify(password, user.PasswordHash))
        return null;

      return GenerateJwt(user);
    }

    private string GenerateJwt(User user)
    {
      var claims = new[]
      {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

      var key = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
      );

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: _config["Jwt:Issuer"],
          audience: _config["Jwt:Audience"],
          claims: claims,
          expires: DateTime.UtcNow.AddMinutes(
              int.Parse(_config["Jwt:ExpiryMinutes"]!)
          ),
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
