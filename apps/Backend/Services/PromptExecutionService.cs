using Backend.Controllers;
using Backend.Data;
using Backend.Models;
using OpenAI.Chat;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Backend.Services
{
    public class PromptExecutionService
    {
        private readonly AppDbContext _db;
        private readonly OpenAIService _openAI;
        private readonly ApplicationService _applicationService;
        private readonly FileTypeService _fileTypeService;
        private readonly RegionService _regionService;
        private readonly UserChatService _userChatService;
        private readonly UserChatContextService _userChatContextService;
        private readonly UserPromptService _userPromptService;



        public PromptExecutionService(AppDbContext db, OpenAIService openAI, ApplicationService applicationService, FileTypeService fileTypeService, RegionService regionService, UserChatService userChatService, UserPromptService userPromptService, UserChatContextService userChatContextService)
        {
            _db = db;
            _openAI = openAI;
            _applicationService = applicationService;
            _fileTypeService = fileTypeService;
            _regionService = regionService;
            _userChatService = userChatService;
            _userPromptService = userPromptService;
            _userChatContextService = userChatContextService;
        }


        public async Task<List<ChatMessage>> BuildContextPrompt(UserChat userChat)
        {

            UserChatContext userChatContext = await _userChatContextService.GetByUserChatId(userChat.UserChatId) ?? throw new BadHttpRequestException("User chat context not found!");

            UserChatContextData userChatContextData = JsonSerializer.Deserialize<UserChatContextData>(userChatContext.UserChatContextData) ?? throw new BadHttpRequestException("User chat context is invalid!");

            List<UserPrompt> userPrompts = await _userPromptService.GetByUserChatIdAsync(userChat.UserChatId, 5);

            Application application =
                await _applicationService.GetById(userChat.ApplicationId)
                ?? throw new BadHttpRequestException("Invalid application!");

            Region region =
                await _regionService.GetById(userChat.RegionId)
                ?? throw new BadHttpRequestException("Invalid region!");

            FileType fileType =
                await _fileTypeService.GetById(userChat.FileTypeId)
                ?? throw new BadHttpRequestException("Invalid file type!");

            var messages = new List<ChatMessage>();

            /* ---------- SYSTEM CONTENT ---------- */

            messages.Add(new SystemChatMessage(
                $"You are an AI assistant for application '{application.ApplicationName}'. {application.PromptText}"
            ));

            messages.Add(new SystemChatMessage(
                $"You are operating in the '{region.RegionName}' region. {region.PromptText}"
            ));

            if (userPrompts.Count > 0)
            {
                messages.Add(new SystemChatMessage("Previous chat summary"));
                foreach (var userPrompt in userPrompts)
                {
                    messages.Add(new SystemChatMessage($"Previous interaction summary: {userPrompt.SummaryText}"));
                }
            }
            
            messages.Add(new SystemChatMessage(
                $"The primary file type is '{fileType.FileTypeName}'. {fileType.PromptText}"
            ));

            /* ---------- FILE CONTENT ---------- */

            messages.Add(new UserChatMessage(
                $"The following is the primary file content. Treat it as read-only context. {userChatContextData.FileName}\n{userChatContextData.FileContent}"
            ));

            if (userChatContextData.ChildFiles != null && userChatContextData.ChildFiles.Length > 0)
            {
                messages.Add(new SystemChatMessage(
                    "The following are supporting child files. Use them as additional context."
                ));

                foreach (var file in userChatContextData.ChildFiles)
                {
                    var childFileType =
                        await _fileTypeService.GetByTypeAndApplicationIdAsync(
                            file.FileType,
                            application.ApplicationId)
                        ?? throw new BadHttpRequestException(
                            $"Invalid child file type: {file.FileType}");

                    messages.Add(new SystemChatMessage(
                        $"Child file type: {childFileType.FileTypeName}. {childFileType.PromptText}"
                    ));

                    messages.Add(new UserChatMessage(
                        $"File: {file.FileName}\n{file.FileContent}"
                    ));
                }
            }
            return messages;
        }

        public async Task<List<ChatMessage>> BuildPrompt(ExecutePromptRequest request)
        {
            UserChat userChat = await _userChatService.GetById(request.UserChatId) ?? throw new BadHttpRequestException("Invalid user chat!");

            List<ChatMessage> messages = new List<ChatMessage>();
            
            messages.AddRange(await BuildContextPrompt(userChat));

            /* ---------- USER PROMPT ---------- */
            if (string.IsNullOrWhiteSpace(request.Prompt))
                throw new BadHttpRequestException("User prompt is required.");

            messages.Add(new UserChatMessage(request.Prompt));

            return messages;
        }

        public async Task<string> GetSummaryOfPromptResponse(string Prompt, string ResponseText)
        {
            List<ChatMessage> messages = new List<ChatMessage>();
            messages.Add(new SystemChatMessage("You summarize assistant responses in 1–2 sentences, preserving key facts."));
            messages.Add(new UserChatMessage($"Prompt:{Prompt}"));
            messages.Add(new UserChatMessage($"ResponseText:{ResponseText}"));
            return await _openAI.ExecutePromptAsync(messages);
        }

        public async Task StorePromptResponse(int UserChatId, string Prompt, string ResponseText)
        {
            await _userPromptService.Create(new UserPrompt
            {
                Prompt = Prompt,
                ResponseText = ResponseText,
                SummaryText = await GetSummaryOfPromptResponse(Prompt, ResponseText),
                UserChatId = UserChatId,
                AddedOn = DateTime.UtcNow,
            });
        }

        public async Task<string> ExecutePromptAsync(ExecutePromptRequest request)
        {
            List<ChatMessage> messages = await BuildPrompt(request);

            string response = await _openAI.ExecutePromptAsync(messages);

            await StorePromptResponse(request.UserChatId, request.Prompt, response);

            return response;
        }

        public IAsyncEnumerable<string> ExecutePromptStreamingAsync(ExecutePromptRequest request)
        {
            return ExecuteInternal(request);

            async IAsyncEnumerable<string> ExecuteInternal(ExecutePromptRequest req)
            {
                List<ChatMessage> messages = await BuildPrompt(req);

                var response = new StringBuilder();

                try
                {
                    await foreach (var chunk in _openAI.ExecutePromptStreamingAsync(messages))
                    {
                        response.Append(chunk);
                        yield return chunk;
                    }
                }
                finally
                {
                    if (response.Length > 0)
                    {
                        await StorePromptResponse(
                            req.UserChatId,
                            req.Prompt,
                            response.ToString()
                        );
                    }
                }
            }
        }
    }
}
