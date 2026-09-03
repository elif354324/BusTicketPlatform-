namespace BusTicketAI.API.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string? PassengerName { get; set; }
        public string? SeatNumber { get; set; }
        public decimal Price { get; set; }
        public bool IsSold { get; set; }

        public int TripId { get; set; }
        public Trip? Trip { get; set; }
    }
}
