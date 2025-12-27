using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class ResponseTypeService
    {
        private readonly AppDbContext _db;

        public ResponseTypeService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ResponseType>> GetAll()
        {
            return await _db.ResponseTypes.ToListAsync();
        }

        public async Task<ResponseType?> GetById(int id)
        {
            return await _db.ResponseTypes.FindAsync(id);
        }

        public async Task<ResponseType?> GetByTypeAsync(string responseType)
        {
            return await _db.ResponseTypes.SingleOrDefaultAsync(a => a.ResponseTypeName == responseType);
        }

        public async Task<ResponseType> Create(ResponseType entity)
        {
            _db.ResponseTypes.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Update(int id, ResponseType input)
        {
            var entity = await _db.ResponseTypes.FindAsync(id);
            if (entity == null) return false;

            entity.ResponseTypeName = input.ResponseTypeName;
            entity.IsActive = input.IsActive;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _db.ResponseTypes.FindAsync(id);
            if (entity == null) return false;

            _db.ResponseTypes.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
