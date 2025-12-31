namespace Backend.Models
{
  public class UserPrompt
  {
    public int UserPromptId { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string ResponseText { get; set; } = null!;
    public string SummaryText { get; set; } = null!;
    public int UserChatId { get; set; }
    public DateTime AddedOn { get; set; }
  }
}
