namespace DrivePal.Models
{
    public class ChatGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ChatMessage> Messages { get; set; }
        public ICollection<UserChatGroup> UserChatGroups { get; set; }
    }

}
