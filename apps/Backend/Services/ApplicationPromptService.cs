using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class ApplicationPromptService
    {
        private readonly AppDbContext _db;

        public ApplicationPromptService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ApplicationPrompt>> GetAll()
        {
            return await _db.ApplicationPrompts.ToListAsync();
        }

        public async Task<ApplicationPrompt?> GetById(int id)
        {
            return await _db.ApplicationPrompts.FindAsync(id);
        }

        public async Task<ApplicationPrompt> Create(ApplicationPrompt entity)
        {
            _db.ApplicationPrompts.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Update(int id, ApplicationPrompt input)
        {
            var entity = await _db.ApplicationPrompts.FindAsync(id);
            if (entity == null) return false;

            entity.ApplicationId = input.ApplicationId;
            entity.PromptText = input.PromptText;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _db.ApplicationPrompts.FindAsync(id);
            if (entity == null) return false;

            _db.ApplicationPrompts.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
