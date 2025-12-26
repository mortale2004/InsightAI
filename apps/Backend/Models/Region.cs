namespace Backend.Models
{
  public class Region
  {
    public int RegionId { get; set; }
    public string RegionName { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
  }
}
