using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrivePal.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        public string StripeId { get; set; }

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        // Navigation properties
        [ForeignKey("Booking")]
        public int? BookingId { get; set; }
        public virtual Booking? Booking { get; set; }
    }
}
