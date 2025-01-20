namespace SquashClubAPI.Models
{
    public class Court
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public decimal LightingCostPerSession { get; set; }
        public CourtStatus Status { get; set; }

    }
}
