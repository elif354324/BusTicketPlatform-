namespace BusTicketAI.API.DTOs
{
    public class TicketCreateDto
    {
        public string PassengerName { get; set; }
        public string SeatNumber { get; set; }
        public decimal Price { get; set; }

        public int TripId { get; set; }
    }
}
