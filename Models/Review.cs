using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrivePal.Models
{
    public class Review
    {
        [Key]
        //Product Properties
        public int ReviewId { get; set; }

     
        [Required]
        [Display(Name = "Your Rating")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0", "5.00")]
        public required decimal Rating { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Leave your review here")]
        public string? ReviewMessage { get; set; }
    
        public DateTime? DateCreated { get; set; }

    


    //Navigational properties
        public string InstructorId { get; set; }

        public virtual Instructor? Instructor { get; set; }

        
        public string LearnerId { get; set; }

        public  Learner? Learner;

    }
}
