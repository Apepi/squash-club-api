namespace SquashClubAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }         
        public User User { get; set; }          
        public int CourtId { get; set; }        
        public Court Court { get; set; }        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public BookingStatus Status { get; set; }
        public decimal Cost { get; set; }
    }
}
