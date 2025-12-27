using Backend.Controllers;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using OpenAI.Chat;
using System.Security.Claims;
using System.Text;

namespace Backend.Services
{
    public class PromptExecutionService
    {
        private readonly AppDbContext _db;
        private readonly OpenAIService _openAI;
        private readonly ApplicationService _applicationService;
        private readonly FileTypeService _fileTypeService;
        private readonly RegionService _regionService;
        private readonly ResponseTypeService _responseTypeService;




        public PromptExecutionService(AppDbContext db, OpenAIService openAI, ApplicationService applicationService, FileTypeService fileTypeService, RegionService regionService, ResponseTypeService responseTypeService)
        {
            _db = db;
            _openAI = openAI;
            _applicationService = applicationService;
            _fileTypeService = fileTypeService;
            _regionService = regionService;
            _responseTypeService = responseTypeService;
        }

        public async Task<string> ExecuteAsync(
            ExecutePromptRequest request
        )
        {
            Application application =
                await _applicationService.GetByNameAsync(request.ApplicationName)
                ?? throw new BadHttpRequestException("Invalid application name!");

            Region region =
                await _regionService.GetByNameAsync(request.RegionName)
                ?? throw new BadHttpRequestException("Invalid region name!");

            FileType fileType =
                await _fileTypeService.GetByTypeAndApplicationIdAsync(request.FileType,application.ApplicationId)
                ?? throw new BadHttpRequestException("Invalid file type!");

            var messages = new List<ChatMessage>();

            /* ---------- SYSTEM CONTEXT ---------- */

            messages.Add(new SystemChatMessage(
                $"You are an AI assistant for application '{application.ApplicationName}'. {application.PromptText}"
            ));

            messages.Add(new SystemChatMessage(
                $"You are operating in the '{region.RegionName}' region. {region.PromptText}"
            ));

            messages.Add(new SystemChatMessage(
                $"The primary file type is '{fileType.FileTypeName}'. {fileType.PromptText}"
            ));

            /* ---------- FILE CONTENT ---------- */

            messages.Add(new UserChatMessage(
                $"Primary File: {request.FileName}\n{request.FileContent}"
            ));

            if (request.ChildFiles != null && request.ChildFiles.Length > 0)
            {
                messages.Add(new SystemChatMessage(
                    "The following are supporting child files. Use them as additional context."
                ));

                foreach (var file in request.ChildFiles)
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

            /* ---------- USER PROMPT ---------- */

            if (string.IsNullOrWhiteSpace(request.Prompt))
                throw new BadHttpRequestException("User prompt is required.");

            messages.Add(new UserChatMessage(request.Prompt));

            /* ---------- RESPONSE FORMAT ENFORCEMENT ---------- */

            ResponseType responseType =
                await _responseTypeService.GetByTypeAsync(Constants.ResponseType.Html)
                ?? throw new BadHttpRequestException("Invalid response type!");

            messages.Add(new SystemChatMessage(
                responseType.ResponseTypeName switch
                {
                    "HTML" => "Respond ONLY with valid HTML. Do not include explanations.",
                    "JSON" => "Respond ONLY with valid JSON. Do not include markdown or comments.",
                    "TEXT" => "Respond in plain text only.",
                    _ => "Respond concisely."
                }
            ));

            /* ---------- EXECUTE OPENAI ---------- */
            var output = await _openAI.ExecutePromptAsync(messages);
            return output;


            // Save response
            //var userPrompt = new UserPrompt
            //{
            //    UserId = 1,
            //    PromptId = 1,
            //    ApplicationId = application.ApplicationId,
            //    ResponseTypeId = application.ApplicationId,
            //    ResponseText = aiResponse,
            //    AddedOn = DateTime.UtcNow
            //};

            //_db.UserPrompts.Add(userPrompt);
            //await _db.SaveChangesAsync();

        }
    }
}
