using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class UserPromptService
    {
        private readonly AppDbContext _db;

        public UserPromptService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<UserPrompt>> GetAll()
        {
            return await _db.UserPrompts.ToListAsync();
        }

        public async Task<UserPrompt?> GetById(int id)
        {
            return await _db.UserPrompts.FindAsync(id);
        }

        public async Task<List<UserPrompt>> GetByUserChatIdAsync(int userChatId, int? limit = null)
        {
            IQueryable<UserPrompt> query = _db.UserPrompts
                                        .Where(up => up.UserChatId == userChatId)
                                        .OrderByDescending(up => up.AddedOn);

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            return await query
                .OrderBy(up => up.AddedOn)
                .ToListAsync();
        }

        public async Task<UserPrompt> Create(UserPrompt entity)
        {
            entity.AddedOn = DateTime.UtcNow;
            _db.UserPrompts.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _db.UserPrompts.FindAsync(id);
            if (entity == null) return false;

            _db.UserPrompts.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
