using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
  public class PromptService
  {
    private readonly AppDbContext _db;

    public PromptService(AppDbContext db)
    {
      _db = db;
    }

    public async Task<List<Prompt>> GetAll()
    {
      return await _db.Prompts.ToListAsync();
    }

    public async Task<Prompt?> GetById(int id)
    {
      return await _db.Prompts.FindAsync(id);
    }

    public async Task<Prompt> Create(Prompt entity)
    {
      _db.Prompts.Add(entity);
      await _db.SaveChangesAsync();
      return entity;
    }

    public async Task<bool> Update(int id, Prompt input)
    {
      var entity = await _db.Prompts.FindAsync(id);
      if (entity == null) return false;

      entity.PromptName = input.PromptName;
      entity.ApplicationId = input.ApplicationId;
      entity.FileTypeId = input.FileTypeId;
      entity.RegionId = input.RegionId;
      entity.PromptText = input.PromptText;

      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Delete(int id)
    {
      var entity = await _db.Prompts.FindAsync(id);
      if (entity == null) return false;

      _db.Prompts.Remove(entity);
      await _db.SaveChangesAsync();
      return true;
    }
  }
}
