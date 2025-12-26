using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
  public class PromptResponseService
  {
    private readonly AppDbContext _db;

    public PromptResponseService(AppDbContext db)
    {
      _db = db;
    }

    public async Task<List<PromptResponse>> GetAll()
    {
      return await _db.PromptResponses.ToListAsync();
    }

    public async Task<PromptResponse?> GetById(int id)
    {
      return await _db.PromptResponses.FindAsync(id);
    }

    public async Task<PromptResponse> Create(PromptResponse entity)
    {
      _db.PromptResponses.Add(entity);
      await _db.SaveChangesAsync();
      return entity;
    }

    public async Task<bool> Delete(int id)
    {
      var entity = await _db.PromptResponses.FindAsync(id);
      if (entity == null) return false;

      _db.PromptResponses.Remove(entity);
      await _db.SaveChangesAsync();
      return true;
    }
  }
}
