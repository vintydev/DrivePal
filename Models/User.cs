using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DrivePal.Models
{
    public class User: IdentityUser
    {
        // Default profile picture URL
        public string ProfilePicture { get; set; } = "/img/profilepictures/defaultprofile.png";

        [Required]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public required string LastName { get; set; }
        public Gender? Gender { get; set; }

        [Required]
        public required string Street { get; set; }
        [Required]
        public required string City { get; set; }
        [Required]
        public required string PostCode { get; set; }
        [Display(Name = "Date of Birth")]
        public required DateOnly DOB { get; set; }

        public bool? isBlocked { get; set; }

        public bool? isExperienced { get; set; }

        [Display(Name = "Joined")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegisteredAt { get; set; }

    }
}
