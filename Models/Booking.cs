namespace DrivePal.Models
{
    public class Booking
    {//Booking = BookingId, BookingDate, Price. Foreign Keys: LessonId, InstructorId, LearnerId, PaymentId. 
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }

        public decimal Price { get; set; }
    }
}
