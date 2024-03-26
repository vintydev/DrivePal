namespace DrivePal.Models
{
    public class Chat
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastMessage { get; set; }
        public int UnreadCount { get; set; }
    }

}
