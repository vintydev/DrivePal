using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrivePal.Models.ViewModels
{
    public class ReportsViewModel
    {
        [Display(Name = "Your Rating")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0", "5.00")]
        public required decimal Rating { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Leave your review here")]
        public string? ReviewMessage { get; set; }

        public DateTime? DateCreated { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
    
        public string? ReportMessage { get; set; }
        [Required]
        public string ReportReason { get; set; }





        //Navigational properties
        public string InstructorId { get; set; }

        public virtual Instructor? Instructor { get; set; }


        public string LearnerId { get; set; }

        public virtual Learner? Learner { get; set; }

  

       //navigation propertties
        public int ReviewId { get; set; }
        public Review Review { get; set; }

       
        public string ReporterId { get; set; }
       

    }
}
  