namespace Backend.Models
{
    public class UserChatContext
    {
        public int UserChatContextId { get; set; }
        public int UserChatId { get; set; }
        public string UserChatContextData { get; set; } = null!;
    }

    public class ChildFile
    {
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public string FileContent { get; set; } = null!; 
    }

    public class UserChatContextData
    {
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public string FileContent { get; set; } = null!;
        public ChildFile[]? ChildFiles { get; set; }
    }
}
