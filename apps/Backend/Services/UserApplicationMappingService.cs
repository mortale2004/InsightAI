using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class UserApplicationMappingService
    {
        private readonly AppDbContext _db;

        public UserApplicationMappingService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<UserApplicationMapping>> GetAll()
        {
            return await _db.UserApplicationMappings.ToListAsync();
        }

        public async Task<UserApplicationMapping?> GetById(int id)
        {
            return await _db.UserApplicationMappings.FindAsync(id);
        }

        public async Task<UserApplicationMapping> Create(UserApplicationMapping entity)
        {
            _db.UserApplicationMappings.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _db.UserApplicationMappings.FindAsync(id);
            if (entity == null) return false;

            _db.UserApplicationMappings.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
