namespace Backend.Models
{
  public class ResponseType
  {
    public int ResponseTypeId { get; set; }
    public string ResponseTypeName { get; set; } = null!;
    public bool IsActive { get; set; }
  }
}
