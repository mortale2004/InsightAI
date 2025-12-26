using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
  public class UserService
  {
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
      _db = db;
    }

    public async Task<List<User>> GetAll()
    {
      return await _db.Users.ToListAsync();
    }

    public async Task<User?> GetById(int id)
    {
      return await _db.Users.FindAsync(id);
    }

    public async Task<User> Create(User entity)
    {
      _db.Users.Add(entity);
      await _db.SaveChangesAsync();
      return entity;
    }

    public async Task<bool> Update(int id, User input)
    {
      var entity = await _db.Users.FindAsync(id);
      if (entity == null) return false;

      entity.FirstName = input.FirstName;
      entity.LastName = input.LastName;
      entity.MiddleName = input.MiddleName;
      entity.Email = input.Email;
      entity.RegionId = input.RegionId;

      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Delete(int id)
    {
      var entity = await _db.Users.FindAsync(id);
      if (entity == null) return false;

      _db.Users.Remove(entity);
      await _db.SaveChangesAsync();
      return true;
    }
  }
}
