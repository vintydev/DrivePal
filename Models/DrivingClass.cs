using System.ComponentModel.DataAnnotations.Schema;

namespace DrivePal.Models
{
    public class DrivingClass
    {
        public int DrivingClassId { get; set; }

        public DateTime DrivingClassStart { get; set; }
        public DateTime DrivingClassEnd { get; set; }
        public decimal Price { get; set; }
        public bool? IsReserved { get; set; }

        ///Navigation properties
        [ForeignKey("Instructor")]
        public string? InstructorId { get; set; }
        public virtual Instructor? Instructor { get; set; }
        [ForeignKey("Learner")]
        public string? LearnerId { get; set; }
        public virtual Learner? Learner { get; set; }
        public virtual Booking? Booking { get; set; }
    }
}
