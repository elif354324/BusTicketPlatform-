namespace BusTicketAI.API.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string?  DepartureCity { get; set; }
        public string? ArrivalCity { get; set; }
        public DateTime DepartureTime { get; set; }

        public decimal Price { get; set; }
        public int BusId { get; set; }
        public Bus? Bus { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
    }
}
