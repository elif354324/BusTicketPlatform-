namespace BusTicketAI.API.DTOs
{
    public class TicketResponse
    {
        public int Id { get; set; }
        public string PassengerName { get; set; }
        public string SeatNumber { get; set; }
        public decimal Price { get; set; }
        public bool IsSold { get; set; }
        public int TripId { get; set; }

        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
    }
}
