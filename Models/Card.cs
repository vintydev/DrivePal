namespace DrivePal.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public string CardHolderName { get; set; }
        public string CardHolderAddress { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpireDate { get; set; }
        public string SecurityCode { get; set; }
    }
}
