namespace DrivePal.Models
    
{
    // Learner inherits from user
    public class Learner: User
    {
        public required string LicenceNumber { get; set; }

        public required Gender Gender { get; set; }

        public required bool isBlocked { get; set; }

        public required bool isExperienced { get; set; }

        public required int lessonCount { get; set; }


    }

    // Enum for drop down list
    public enum Gender
    {
        Male,
        Female,
        PreferNotToSay,
        NonBinary
    }
}
