using SquashClubAPI.Data;
using SquashClubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SquashClubAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourtService _courtService;  // For availability checks

        public BookingService(ApplicationDbContext context, ICourtService courtService)
        {
            _context = context;
            _courtService = courtService;
        }

        public async Task<Booking> CreateBooking(int userId, int courtId, DateTime startTime, DateTime endTime)
        {
            var isAvailable = await _courtService.IsCourtAvailable(courtId, startTime, endTime);
            if (!isAvailable)
            {
                return null;
            }

            var booking = new Booking
            {
                UserId = userId,
                CourtId = courtId,
                StartTime = startTime,
                EndTime = endTime,
                Status = BookingStatus.Confirmed,
                Cost = 15.00m  // per session which is set at 45mins
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> CancelBooking(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return false;
            }

            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Booking> GetBookingById(int bookingId)
        {
            return await _context.Bookings.FindAsync(bookingId);
        }

        public async Task<IEnumerable<Booking>> GetUserBookings(int userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetCourtBookings(int courtId, DateTime date)
        {
            return await _context.Bookings
                .Where(b => b.CourtId == courtId
                    && b.StartTime.Date == date.Date)
                .ToListAsync();
        }




    }
}
