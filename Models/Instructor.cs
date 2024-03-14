using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        
        [Display(Name = "Average Rating")]
        [Column(TypeName = "decimal(3,1)")]
        public decimal TotalRating { get; set; }
        
        
        // Questionnaire Metrics
        public decimal AveragePricePerLesson { get; set; }
        
        

        //navigational property
        public virtual ICollection<Review>? Reviews { get; set; }
        public List<DrivingClass>? DrivingClasses { get; set; }

        public string GetPostcode()
        {
            return this.PostCode;
        }

    }

}
