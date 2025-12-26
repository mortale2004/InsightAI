namespace Backend.Models
{
  public class Prompt
  {
    public int PromptId { get; set; }
    public string PromptName { get; set; } = null!;
    public int ApplicationId { get; set; }
    public int FileTypeId { get; set; }
    public int RegionId { get; set; }
    public string PromptText { get; set; } = null!;
  }
}
