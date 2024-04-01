namespace DrivePal.Models.ViewModels
{
    public class LearnerBookingsViewModel
    {
        public List<BookingDetail> Bookings { get; set; }
    }

    public class BookingDetail
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal Price { get; set; }
        public DateTime DrivingClassStart { get; set; }
        public DateTime DrivingClassEnd { get; set; }

        public string? InstructorFirstName { get; set; }
        public string? InstructorLastName { get; set; } // Added property for the instructor's last name
        public string? LearnerLastName { get; set; } // For instructor's view

        // Add any other details you want to show in the view
    }
}
