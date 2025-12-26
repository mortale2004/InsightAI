namespace Backend.Models
{
  public class FileType
  {
    public int FileTypeId { get; set; }
    public string FileTypeName { get; set; } = null!;
    public string? Description { get; set; }
    public string PromptText { get; set; } = null!;
  }
}
