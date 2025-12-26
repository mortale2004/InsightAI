namespace Backend.Models
{
  public class PromptResponse
  {
    public int PromptResponseId { get; set; }
    public int PromptId { get; set; }
    public int ResponseTypeId { get; set; }
  }
}
