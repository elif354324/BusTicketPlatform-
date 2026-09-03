using Microsoft.AspNetCore.Mvc;
using BusTicketAI.API.Models;
using BusTicketAI.API.Repositories;
using BusTicketAI.API.DTOs;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace BusTicketAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly IGenericRepository<Trip> _tripRepository;

        public TripsController(IGenericRepository<Trip> tripRepository)
        {
            _tripRepository = tripRepository;
        }

        // OBİLET MANTIKLI ARAMA MOTORU EKRANI
        [HttpGet("search")]
        public async Task<IActionResult> SearchTrips([FromQuery] string from, [FromQuery] string to, [FromQuery] DateTime date)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                return BadRequest("Kalkış ve Varış şehirleri boş bırakılamaz.");
            }

            // Veritabanından seferleri, onlara bağlı Otobüs (Bus) ve Bilet (Tickets) listesiyle beraber çek
            // Include (Join) işlemi yapıyoruz ki otobüsün firmasını ve satılan biletleri görebilelim.
            var trips = await _tripRepository.GetAllWithIncludesAsync(t => t.Bus, t => t.Tickets);

            // Gelen veriyi Şehir ve Tarih bazında filtrele
            var filteredTrips = trips.Where(t =>
                t.DepartureCity.ToLower() == from.ToLower() &&
                t.ArrivalCity.ToLower() == to.ToLower() &&
                t.DepartureTime.Date == date.Date // Sadece kullanıcının seçtiği güne ait seferler
            ).ToList();

            if (!filteredTrips.Any())
            {
                return NotFound("Aradığınız kriterlere uygun sefer bulunamadı.");
            }

            // Filtrelenmiş seferleri kullanıcının göreceği DTO'ya dönüştür
            var tripDtos = filteredTrips.Select(t => new TripListDto
            {
                Id = t.Id,
                DepartureCity = t.DepartureCity,
                ArrivalCity = t.ArrivalCity,
                DepartureTime = t.DepartureTime,
                Price = t.Price,

                // Bus modelinden gelen veriler (Bus modelindeki özellik isimlerine göre düzenleyebilirsin)
                CompanyName = t.Bus?.CompanyName ?? "Bilinmeyen Firma",
                BusType = t.Bus?.BusType ?? "Bilinmeyen Tip",

                // BOŞ KOLTUK HESAPLAMASI: 
                // Otobüsün kapasitesi (Örn: 30) eksi satılan (IsSold = true) bilet sayısı
                AvailableSeatCount = 30 - (t.Tickets != null ? t.Tickets.Count(x => x.IsSold) : 0)
            })
            .OrderBy(t => t.DepartureTime) // Seferleri sabah saatlerinden akşama doğru sırala
            .ToList();

            return Ok(tripDtos);
        }

        // TEST VERİSİ ÜRETİCİSİ (DATA SEEDER)
        [HttpPost("generate-fake-data")]
        public async Task<IActionResult> GenerateFakeTrips()
        {
            var sehirler = new[] { "Manisa", "Ayvalık", "İzmir", "İstanbul", "Ankara", "Bursa", "Antalya", "Eskişehir" };
            var firmalar = new[] { "Kamil Koç", "Metro Turizm", "Pamukkale", "Varan", "Ali Osman Ulusoy" };
            var tipler = new[] { "2+1 VIP", "2+2 Standart" };
            var random = new Random();

            int uretilenSeferSayisi = 0;

            for (int i = 0; i < 50; i++) // 50 adet rastgele sefer üretecek
            {
                // Rastgele iki farklı şehir seç
                string kalkis = sehirler[random.Next(sehirler.Length)];
                string varis;
                do
                {
                    varis = sehirler[random.Next(sehirler.Length)];
                } while (kalkis == varis); // Kalkış ve varış aynı olmasın

                // Rastgele bir tarih ve saat (Önümüzdeki 30 gün içinde)
                DateTime rastgeleTarih = DateTime.Now.AddDays(random.Next(0, 30)).AddHours(random.Next(6, 23));

                var yeniSefer = new Trip
                {
                    DepartureCity = kalkis,
                    ArrivalCity = varis,
                    DepartureTime = rastgeleTarih,
                    Price = random.Next(300, 900), // 300₺ ile 900₺ arası rastgele fiyat

                    // Bus modelini de anında yaratıp bağlıyoruz
                    Bus = new Bus
                    {
                        CompanyName = firmalar[random.Next(firmalar.Length)],
                        BusType = tipler[random.Next(tipler.Length)],
                        PlateNumber = "45 TEST " + random.Next(100, 999)
                    }
                };

                await _tripRepository.AddAsync(yeniSefer);
                uretilenSeferSayisi++;
            }

            await _tripRepository.SaveChangesAsync();

            return Ok($"Başarılı! Veritabanına {uretilenSeferSayisi} adet rastgele sefer ve otobüs eklendi.");
        }
    }
}