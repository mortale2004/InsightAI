namespace Backend.Models
{
  public class ApplicationPrompt
  {
    public int ApplicationPromptId { get; set; }
    public int ApplicationId { get; set; }
    public string PromptText { get; set; } = null!;
  }
}
