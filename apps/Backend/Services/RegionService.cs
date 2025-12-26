using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
  public class RegionService
  {
    private readonly AppDbContext _db;

    public RegionService(AppDbContext db)
    {
      _db = db;
    }

    public async Task<List<Region>> GetAll()
    {
      return await _db.Regions.ToListAsync();
    }

    public async Task<Region?> GetById(int id)
    {
      return await _db.Regions.FindAsync(id);
    }

    public async Task<Region> Create(Region entity)
    {
      _db.Regions.Add(entity);
      await _db.SaveChangesAsync();
      return entity;
    }

    public async Task<bool> Update(int id, Region input)
    {
      var entity = await _db.Regions.FindAsync(id);
      if (entity == null) return false;

      entity.RegionName = input.RegionName;
      entity.Description = input.Description;
      entity.IsActive = input.IsActive;

      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Delete(int id)
    {
      var entity = await _db.Regions.FindAsync(id);
      if (entity == null) return false;

      _db.Regions.Remove(entity);
      await _db.SaveChangesAsync();
      return true;
    }
  }
}
