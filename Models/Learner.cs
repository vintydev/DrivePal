using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;


namespace DrivePal.Models
    
{
    // Learner inherits from user
    public class Learner: User
    {
        
        public string? LicenceNumber { get; set; }

        public Gender? Gender { get; set; }

        public  bool? isBlocked { get; set; }

        public bool? isExperienced { get; set; }

        public int? lessonCount { get; set; }

        //Navigational property
        public List<Review>? Reviews { get; set; }
        public virtual ICollection<Card>? Cards { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }

    }

    // Enum for drop down list
    public enum Gender
    {
        Male,
        Female,
        [Display(Name = "Prefer Not to Say")]
        PreferNotToSay,
        [Display(Name = "Non-Binary")]
        NonBinary
    }
}
