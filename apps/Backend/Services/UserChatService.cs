using Backend.Controllers;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Backend.Services
{
    public class UserChatService
    {
        private readonly AppDbContext _db;
        private readonly ApplicationService _applicationService;
        private readonly FileTypeService _fileTypeService;
        private readonly RegionService _regionService;
        private readonly UserChatContextService _userChatContextService;

        public UserChatService(AppDbContext db, ApplicationService applicationService, FileTypeService fileTypeService, RegionService regionService, UserChatContextService userChatContextService)
        {
            _db = db;
            _applicationService = applicationService;
            _fileTypeService = fileTypeService;
            _regionService = regionService;
            _userChatContextService = userChatContextService;
        }

        public async Task<List<UserChat>> GetAll()
        {
            return await _db.UserChats.ToListAsync();
        }

        public async Task<UserChat?> GetById(int id)
        {
            return await _db.UserChats.FindAsync(id);
        }

        public async Task<UserChat> Create(UserChatRequest request)
        {
            Application application =
                await _applicationService.GetByNameAsync(request.ApplicationName)
                ?? throw new BadHttpRequestException("Invalid application name!");

            Region region =
                await _regionService.GetByNameAsync(request.RegionName)
                ?? throw new BadHttpRequestException("Invalid region!");

            FileType fileType =
                await _fileTypeService.GetByTypeAndApplicationIdAsync(request.FileType, application.ApplicationId)
                ?? throw new BadHttpRequestException("Invalid file type!");

            await _db.SaveChangesAsync();

            UserChat userChat = new UserChat
            {
                FileTypeId = fileType.FileTypeId,
                ApplicationId = application.ApplicationId,
                RegionId = region.RegionId,
                AddedOn = DateTime.UtcNow,
                UserChatName = request.FileName,
                UserId = request.UserId,
            };
            _db.UserChats.Add(userChat);


            await _db.SaveChangesAsync();

            await _userChatContextService.Create(new UserChatContext
            {
                UserChatContextData = JsonSerializer.Serialize<UserChatContextData>(request),
                UserChatId = userChat.UserChatId,
            });

            return userChat;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _db.UserChats.FindAsync(id);
            if (entity == null) return false;

            _db.UserChats.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
