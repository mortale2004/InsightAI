using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Services
{
  public class PromptExecutionService
  {
    private readonly AppDbContext _db;
    private readonly OpenAIService _openAI;

    public PromptExecutionService(AppDbContext db, OpenAIService openAI)
    {
      _db = db;
      _openAI = openAI;
    }

    public async Task<string> ExecuteAsync(
        int promptId,
        int responseTypeId,
        int applicationId,
        ClaimsPrincipal user
    )
    {
      var userId = int.Parse(
          user.FindFirstValue(ClaimTypes.NameIdentifier)!
      );

      var prompt = await _db.Prompts
          .FirstOrDefaultAsync(x => x.PromptId == promptId);

      if (prompt == null)
        throw new Exception("Prompt not found");

      // Execute OpenAI
      var aiResponse = await _openAI.ExecutePromptAsync(prompt.PromptText);

      // Save response
      var userPrompt = new UserPrompt
      {
        UserId = userId,
        PromptId = promptId,
        ApplicationId = applicationId,
        ResponseTypeId = responseTypeId,
        ResponseText = aiResponse,
        AddedOn = DateTime.UtcNow
      };

      _db.UserPrompts.Add(userPrompt);
      await _db.SaveChangesAsync();

      return aiResponse;
    }
  }
}
