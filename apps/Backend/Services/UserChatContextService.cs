using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class UserChatContextService
    {
        private readonly AppDbContext _db;

        public UserChatContextService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<UserChatContext>> GetAll()
        {
            return await _db.UserChatContexts.ToListAsync();
        }

        public async Task<UserChatContext?> GetById(int id)
        {
            return await _db.UserChatContexts.FindAsync(id);
        }

        public async Task<UserChatContext?> GetByUserChatId(int UserChatId)
        {
            return await _db.UserChatContexts.SingleOrDefaultAsync((u)=> u.UserChatId == UserChatId);
        }

        public async Task<UserChatContext> Create(UserChatContext entity)
        {
            _db.UserChatContexts.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _db.UserChatContexts.FindAsync(id);
            if (entity == null) return false;

            _db.UserChatContexts.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
