namespace Backend.Models
{
  public class Application
  {
    public int ApplicationId { get; set; }
    public string ApplicationName { get; set; } = null!;
    public string PromptText { get; set; } = null!;
  }
}
