namespace DrivePal.Models
{ 
    public class UserChatGroup
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int ChatGroupId { get; set; }
        public ChatGroup ChatGroup { get; set; }
    }
}
