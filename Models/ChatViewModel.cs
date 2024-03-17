using DrivePal.Models;

public class ChatViewModel
{
    public string OtherUserName { get; set; }
    public string RecipientId { get; set; }
    public List<ChatMessage> Messages { get; set; }
}