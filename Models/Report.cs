using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrivePal.Models
{
    public class Report
    {
        [Key]
        //Product Properties
        public int ReportId { get; set; }


        
        [DataType(DataType.MultilineText)]
     

        public string? ReportMessage { get; set; }
        [Required]
        public string ReportReason { get; set; }

        public DateTime? DateCreated { get; set; }

        public bool ? isProccessed { get; set; }


       
     



        //Navigational properties
        public string InstructorId { get; set; }

        public virtual Instructor? Instructor { get; set; }


        public string ReporterId { get; set; }

    

        public int ReviewId { get; set; }
        public virtual Review? Review { get; set;}

    }
}
    


