namespace Backend.Models
{
    public class UserChat
    {
        public int UserChatId { get; set; }
        public int UserId { get; set; }
        public string UserChatName { get; set; } = null!;
        public int ApplicationId { get; set; }
        public int RegionId { get; set; }
        public int FileTypeId { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
