using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
  public class ApplicationService
  {
    private readonly AppDbContext _db;

    public ApplicationService(AppDbContext db)
    {
      _db = db;
    }

    public async Task<List<Application>> GetAll()
    {
      return await _db.Applications.ToListAsync();
    }

    public async Task<Application?> GetById(int id)
    {
      return await _db.Applications.FindAsync(id);
    }

    public async Task<Application> Create(Application entity)
    {
      _db.Applications.Add(entity);
      await _db.SaveChangesAsync();
      return entity;
    }

    public async Task<bool> Update(int id, Application input)
    {
      var entity = await _db.Applications.FindAsync(id);
      if (entity == null) return false;

      entity.ApplicationName = input.ApplicationName;
      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Delete(int id)
    {
      var entity = await _db.Applications.FindAsync(id);
      if (entity == null) return false;

      _db.Applications.Remove(entity);
      await _db.SaveChangesAsync();
      return true;
    }
  }
}
