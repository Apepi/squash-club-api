using SquashClubAPI.Models;

namespace SquashClubAPI.Services
{
    public interface IBookingService
    {
        Task<Booking> CreateBooking(int userId, int courtId, DateTime startTime, DateTime endTime);
        Task<bool> CancelBooking(int bookingId);
        Task<Booking> GetBookingById(int bookingId);
        Task<IEnumerable<Booking>> GetUserBookings(int userId);
        Task<IEnumerable<Booking>> GetCourtBookings(int courtId, DateTime date);
    }
}
