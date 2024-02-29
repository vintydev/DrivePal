using System.ComponentModel.DataAnnotations;

namespace DrivePal.Models.ViewModels
{
    public class LearnerSeeOwnDetails
    {
        [Display(Name = "Email Address")]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string Street { get; set; }
        [Required]
        public required string City { get; set; }
        [Required]
        public required string PostCode { get; set; }
        [Display(Name = "Date of Birth")]
        public required DateOnly DOB { get; set; }
        [Required]
        public string? LicenceNumber { get; set; }

        public Gender? Gender { get; set; }

        public bool? isBlocked { get; set; }

        public bool? isExperienced { get; set; }

        public int? lessonCount { get; set; }


    }
}
