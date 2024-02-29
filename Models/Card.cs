//using Stripe;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrivePal.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public string CardHolderName { get; set; }
        public string CardHolderAddress { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpireDate { get; set; }
        public string SecurityCode { get; set; }

        [ForeignKey("User")]
        public string? LearnerId { get; set; }
        public virtual Learner? Learner { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
    }
}
