using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace DrivePal.Models
{
    public class Questionnaire
    {
        [Key]
        public int QuestionnaireId { get; set; }
        
       // Error message purposes
        public int? DayIndex { get; set; }
        
        public int? GoalIndex { get; set; }
        
        public int? TraitIndex { get; set; }

        [Range(0.01, 100, ErrorMessage = "Price must be above £0.00 & Below £100.00.")]
        [Display(Name = "Enter minimum price per lesson.")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "You must enter a minimum price.")]
        public decimal MinPrice { get; set; }

        [Range(20.00, 100, ErrorMessage ="Price must be above £20.00 & Below £100.00"),]
        [Display(Name = "Enter maximum price per lesson.")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "You must enter a maximum price")]
        public decimal MaxPrice { get; set; }

        [Display(Name = "How many lessons have you had?")]
        [Required(ErrorMessage = "You must enter your driving experience.")]
        public DrivingStatus DrivingStatus { get; set; }

        [Display(Name = "What are some traits you look for in an instructor? \b Please select all that apply.")]
        [Required(ErrorMessage = "You must select at least one trait.")]
        public List<string> TeachingTraits { get; set; } =
        [
            "Assertive", "Patient", "Calm", "Experienced", "Empathetic", "Talkative", "Funny", "Straight to the point"
        ];

        [Display(Name = "What are some driving goals? \b Please select all that apply.")]
        [Required(ErrorMessage = "You must select at least one goal.")]
        public List<string> DrivingGoals { get; set; } =
            ["Pass quickly", "Gain driving confidence", "Motorway skills"];

        // Static list, won't ever change
        [Display(Name = "Select which type of lessons you would like.")]
        [Required(ErrorMessage = "You must select a lesson type.")]
        public string[] TeachingType { get; set; } = [ "Manual", "Automatic" ];
        
        
        [Display(Name = "What days of the week are you available? \n Please select all that apply.")]
        [Required(ErrorMessage = "You must select at least one day.")]
        public List<string> AvailableDaysOf { get; set; } =
            ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

        // Static list, won't ever change
        [Display(Name = "What time of day would you prefer?")]
        [Required(ErrorMessage = "You must select at least one timeframe.")]
        public string[] TimeOfDay { get; set; } = ["Morning", "Afternoon", "Evening"];

        // Static list, won't ever change
        [Display(Name ="How many minutes per lesson?")]
        [Required(ErrorMessage = "You must select at least one option.")]
        public int[] LessonDuration { get; set; } = [30, 60, 90, 120];
        
        public bool IsFinished { get; set; }

        // Nullable to prevent datetime conversions
        public DateTime? DateCompleted { get; set; }

        // Navigational Propertiessss
        [ForeignKey("Learner")]
        public string? LearnerId { get; set; }
        public Learner? Learner { get; set; }
    }

    public enum DrivingStatus
    {
        [Display(Name = "0 Lessons")]
        [Description("Never had a lesson")]
        NeverDrove,

        [Display(Name = "1-3 Lessons")]
        [Description("Had a lesson or two")]
        Inexperienced,

        [Display(Name = "10+ Lessons")]
        Moderate,

        [Display(Name = "30+ Lessons")]
        [Description("Experienced driver, could pass soon.")]
        Experienced
    }
    
}
