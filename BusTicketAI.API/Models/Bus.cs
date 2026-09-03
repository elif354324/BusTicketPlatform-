namespace BusTicketAI.API.Models
{
    public class Bus
    {
        public int Id { get; set; }
        public string? PlateNumber { get; set; }
        public int Capacity { get; set; }
        public string? CompanyName { get; set; }
        public string? BusType { get; set; }

        public ICollection<Trip>? Trips { get; set; }
    }
}
