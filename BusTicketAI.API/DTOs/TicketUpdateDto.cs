namespace BusTicketAI.API.DTOs
{
    public class TicketUpdateDto
    {
        public string PassengerName { get; set; }
        public string SeatNumber { get; set; }
        public decimal Price { get; set; }
    }
}
