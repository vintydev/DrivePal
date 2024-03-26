using System.ComponentModel.DataAnnotations.Schema;

namespace DrivePal.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public bool Read { get; set; }

        [NotMapped]
        public string SenderName { get; set; }
    }

}
