using Microsoft.AspNetCore.Mvc;
using BusTicketAI.API.Models;
using BusTicketAI.API.Repositories;
using BusTicketAI.API.DTOs;
using System.Threading.Tasks;

namespace BusTicketAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IGenericRepository<Ticket> _ticketRepository;

        public TicketsController(IGenericRepository<Ticket> ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _ticketRepository.GetAllWithIncludesAsync(t => t.Trip);
            var ticketDtos = tickets.Select(t => new TicketResponse
            {
                Id = t.Id,
                PassengerName = t.PassengerName,
                SeatNumber = t.SeatNumber,
                Price = t.Price,
                IsSold = t.IsSold,
                TripId = t.TripId,
                DepartureCity = t.Trip.DepartureCity,
                ArrivalCity= t.Trip.ArrivalCity,
            }).ToList();
            return Ok(ticketDtos);
        }

        [HttpPost]
        public async Task<IActionResult> AddTicket(TicketCreateDto ticketDto)
        {

            var newTicket = new Ticket
            {
                PassengerName = ticketDto.PassengerName,
                SeatNumber = ticketDto.SeatNumber,
                Price = ticketDto.Price,
                TripId = ticketDto.TripId,

                IsSold = true
            };
            await _ticketRepository.AddAsync(newTicket);
            await _ticketRepository.SaveChangesAsync();
            return Ok($"Sayın {ticketDto.PassengerName}, biletiniz güvenli bir şekilde kesildi!");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
           
            var ticket = await _ticketRepository.GetAsync(t => t.Id == id, t => t.Trip);

            if (ticket == null)
            {
                return NotFound($"{id} numaralı bilet sistemde bulunamadı.");
            }

            var ticketDto = new TicketResponse
            {
                Id = ticket.Id,
                PassengerName = ticket.PassengerName,
                SeatNumber = ticket.SeatNumber,
                Price = ticket.Price,
                IsSold = ticket.IsSold,
                TripId = ticket.TripId,

                DepartureCity = ticket.Trip.DepartureCity,
                ArrivalCity = ticket.Trip.ArrivalCity
            };

            return Ok(ticketDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

            if(ticket == null)
            {
                return NotFound($"Hata: {id} numaralı bilet bulunamadı. Zaten iptal edilmiş olabilir.");
            }

            _ticketRepository.Delete(ticket);
            await _ticketRepository.SaveChangesAsync();

            return Ok($"Sayın {ticket.PassengerName}, biletiniz başarıyla iptal edildi");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, TicketUpdateDto updateDto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

            if (ticket == null)
            {
                return NotFound($"{id} numaralı bilet bulunamadı, güncelleme yapılamaz.");
            }

            ticket.PassengerName = updateDto.PassengerName;
            ticket.SeatNumber = updateDto.SeatNumber;
            ticket.Price = updateDto.Price;

            _ticketRepository.Update(ticket);
            await _ticketRepository.SaveChangesAsync();

            return Ok($"Sayın {ticket.PassengerName}, bilet bilgileriniz başarıyla güncellendi.");
        }
    }
}