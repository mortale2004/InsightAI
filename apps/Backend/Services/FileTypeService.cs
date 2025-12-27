using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;

namespace Backend.Services
{
    public class FileTypeService
    {
        private readonly AppDbContext _db;

        public FileTypeService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<FileType>> GetAll()
        {
            return await _db.FileTypes.ToListAsync();
        }

        public async Task<FileType?> GetById(int id)
        {
            return await _db.FileTypes.FindAsync(id);
        }

        public async Task<FileType?> GetByTypeAndApplicationIdAsync(string fileTypeName, int applicationId)
        {
            return await _db.FileTypes
                   .AsNoTracking()
                   .FirstOrDefaultAsync(f =>
                       f.FileTypeName == fileTypeName &&
                       f.ApplicationId == applicationId);
        }

        public async Task<FileType> Create(FileType entity)
        {
            _db.FileTypes.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Update(int id, FileType input)
        {
            var entity = await _db.FileTypes.FindAsync(id);
            if (entity == null) return false;

            entity.FileTypeName = input.FileTypeName;
            entity.Description = input.Description;
            entity.ApplicationId = input.ApplicationId;
            entity.PromptText = input.PromptText;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _db.FileTypes.FindAsync(id);
            if (entity == null) return false;

            _db.FileTypes.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
