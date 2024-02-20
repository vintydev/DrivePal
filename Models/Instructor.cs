using System.ComponentModel.DataAnnotations;

namespace DrivePal.Models
{
    public class Instructor : User
    {
        [Display(Name = "License Number")]
        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "The License Number must be exactly 16 characters.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "The License Number must contain only letters and numbers.")]
        public string? LicenceNumber { get; set; }

        public Gender? Gender { get; set; }

        public bool? isApproved { get; set; }

        public bool? isBlocked { get; set; }


    }

}
