using System.ComponentModel.DataAnnotations;

namespace DrivePal.Models
{
    public class Staff :User
    {
        public WorkType WorkType { get; set; }

    }

    public enum WorkType
    {
        [Display(Name = "Part time")]
        PartTime,
        [Display(Name = "Full time")]
        FullTime
    }
}
