namespace DrivePal.Models
    
{

    public class Learner:User
    {
        public required string LicenceNumber { get; set; }

        public  required Gender Gender { get; set; }
        public bool isBlocked { get; set; }



    }
public enum Gender
{
    Male,
    Female,
    PreferNotToSay,
    NonBinary
}
}
