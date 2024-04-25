namespace DrivePal.Models.ViewModels
{
    public class InstructorsDetailsViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ProfilePicture { get; set; }

        public string PostCode { get; set; }
             
        public List<string>? InstructorDaysAvailable { get; set; }
        
        public List<string>? InstructorTimeAvaiable { get; set; }
        
        public decimal? AveragePrice { get; set; }
        
        public decimal TotalRating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
