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
        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "1.0", "79228162514264337593543950335")]
        public decimal Rating { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string? ReviewMessage { get; set; }
    
        public DateTime? DateCreated { get; set; }


     
    }
}
