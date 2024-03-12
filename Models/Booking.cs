using System.ComponentModel.DataAnnotations.Schema;

namespace DrivePal.Models
{
    public class Booking
    {//Booking = BookingId, BookingDate, Price. Foreign Keys: LessonId, InstructorId, LearnerId, PaymentId. 
        public int BookingId { get; set; }

        public DateTime BookingDate { get; set; }

        public decimal Price { get; set; }

        ///Navigation properties
        [ForeignKey("Instructor")]
        public string? InstructorId { get; set; }
        public virtual Instructor? Instructor { get; set; }
        [ForeignKey("Learner")]
        public string? LearnerId { get; set; }
        public virtual Learner? Learner { get; set; }
        public virtual Payment? Payment { get; set; }
        [ForeignKey("DrivingClass")]
        public int? DrivingClassId { get; set; }
        public virtual DrivingClass? DrivingClass { get; set; }
    }
}
