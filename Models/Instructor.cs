namespace DrivePal.Models
{
    public class Instructor:User
    {  
         public required string LicenceNumber { get; set; }

         public required Gender Gender { get; set; }

        public bool isApproved { get; set; }

        public bool isBlocked { get; set; }





    }

}
