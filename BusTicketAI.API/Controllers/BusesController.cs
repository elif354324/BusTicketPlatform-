using Microsoft.AspNetCore.Mvc;
using BusTicketAI.API.Models;
using BusTicketAI.API.Repositories;
using System.Threading.Tasks;

namespace BusTicketAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        // Yazdığımız Generic Repository'i içeri alıyoruz
        private readonly IGenericRepository<Bus> _busRepository;

        public BusesController(IGenericRepository<Bus> busRepository)
        {
            _busRepository = busRepository;
        }

        // GET İstediği: Tüm otobüsleri listeler
        [HttpGet]
        public async Task<IActionResult> GetAllBuses()
        {
            var buses = await _busRepository.GetAllAsync();
            return Ok(buses);
        }

        // POST İsteği: Sisteme yeni bir otobüs ekler
        [HttpPost]
        public async Task<IActionResult> AddBus(Bus bus)
        {
            await _busRepository.AddAsync(bus);
            await _busRepository.SaveChangesAsync(); // Değişiklikleri kaydetmeyi unutmuyoruz!

            return Ok("Otobüs sisteme başarıyla eklendi!");
        }

        // PUT İsteği: Var olan bir otobüsü günceller
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBus(int id, Bus bus)
        {
            if (id != bus.Id)
            {
                return BadRequest("URL'deki ID ile gönderilen otobüsün ID'si uyuşmuyor!");
            }

            _busRepository.Update(bus);
            await _busRepository.SaveChangesAsync();

            return Ok("Otobüs bilgileri başarıyla güncellendi.");
        }

        // DELETE İsteği: Sistemden bir otobüs siler
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBus(int id)
        {
            var bus = await _busRepository.GetByIdAsync(id);
            if (bus == null)
            {
                return NotFound("Silinmek istenen otobüs bulunamadı.");
            }

            _busRepository.Delete(bus);
            await _busRepository.SaveChangesAsync();

            return Ok("Otobüs sistemden başarıyla silindi.");
        }
    }
}