using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrivePal.Models.ViewModels
{
    public class InstructorEditOwnDetails
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
        [Display(Name = "License Number")]
        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "The License Number must be exactly 16 characters.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "The License Number must contain only letters and numbers.")]
        public string? LicenceNumber { get; set; }

        public Gender? Gender { get; set; }

        public bool? isApproved { get; set; }

        public bool? isBlocked { get; set; }
        [Display(Name = "Total Rating")]
        [Column(TypeName = "decimal(3,1)")]

        public decimal TotalRating { get; set; }
    }
}
