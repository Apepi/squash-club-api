using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquashClubAPI.Models;
using SquashClubAPI.Services;
using System.Security.Claims;

namespace SquashClubAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ITransactionService _transactionService;

        public BookingController(IBookingService bookingService, ITransactionService transactionService)
        {
            _bookingService = bookingService;
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var booking = await _bookingService.CreateBooking(userId, request.CourtId, request.StartTime, request.EndTime);

            if (booking == null)
                return BadRequest("Court not available for selected time slot");

            // Process payment for booking
            await _transactionService.ProcessPayment(userId, booking.Cost, $"Booking for Court {request.CourtId}");

            return Ok(booking);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBookings()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var bookings = await _bookingService.GetUserBookings(userId);
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _bookingService.GetBookingById(id);
            if (booking == null)
                return NotFound();
            return Ok(booking);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var success = await _bookingService.CancelBooking(id);
            if (!success)
                return NotFound();
            return Ok();
        }
    }

    public class CreateBookingRequest
    {
        public int CourtId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}