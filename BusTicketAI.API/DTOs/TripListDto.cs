namespace BusTicketAI.API.DTOs
{
    public class TripListDto
    {
        public int Id { get; set; }
        public DateTime DepartureTime { get; set; }
        public string? DepartureCity { get; set; }
        public string? ArrivalCity { get; set; }
        public decimal Price { get; set; }

        // Otobüs (Bus) Tablosundan Gelecekler
        public string? CompanyName { get; set; } // Örn: "Metro Turizm"
        public string? BusType { get; set; } // Örn: "2+1 VIP"

        // Dinamik Hesaplanacak Alan
        public int AvailableSeatCount { get; set; }
    }
}