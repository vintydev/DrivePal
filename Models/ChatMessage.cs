namespace DrivePal.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }


        public int GroupId { get; set; }
        public ChatGroup Group { get; set; }

    }

}
