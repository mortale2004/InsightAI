namespace Backend.Models
{
  public class UserPrompt
  {
    public int UserPromptId { get; set; }
    public int UserId { get; set; }
    public string ResponseText { get; set; } = null!;
    public int ResponseTypeId { get; set; }
    public int PromptId { get; set; }
    public int ApplicationId { get; set; }
    public DateTime AddedOn { get; set; }
  }
}
