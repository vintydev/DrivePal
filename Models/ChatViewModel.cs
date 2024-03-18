using DrivePal.Models;

public class ChatViewModel
{
    public string CurrentUserName { get; set; }
    public string OtherUserName { get; set; }
    public string RecipientId { get; set; }
    public string GroupName { get; set; }
    public List<ChatMessage> Messages { get; set; }
    public Dictionary<string, string> SenderNames { get; set; }
}