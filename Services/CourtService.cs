using SquashClubAPI.Data;
using SquashClubAPI.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace SquashClubAPI.Services
{
    public class CourtService : ICourtService
    {
        private readonly ApplicationDbContext _context;

        public CourtService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Court>> GetAllCourts()
        {
            return await _context.Courts.ToListAsync();
        }

        public  async Task<Court> GetCourtById(int id)
        {
            return await _context.Courts.FindAsync(id);
        }

        public async Task<bool> UpdateCourtStatus(int id, CourtStatus status)
        {
            var court = await _context.Courts.FindAsync(id);
            if (court == null)
                return false;

            court.Status = status;
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> IsCourtAvailable(int id, DateTime startTime, DateTime endTime)
        {
            // Check if court is Occupied or under Maintenance 
            var court = await _context.Courts.FindAsync(id);
            if (court == null || court.Status == CourtStatus.Maintenance)
                return false;

            // Check for overlapping bookings
            var existingBooking = await _context.Bookings
                .AnyAsync(b => b.CourtId == id
                        && b.Status != BookingStatus.Cancelled
                        && ((startTime >= b.StartTime && startTime < b.EndTime)
                        || (endTime > b.StartTime && endTime <= b.EndTime)));

            return !existingBooking;
        }

        public async Task<Court> CreateCourt(string name, decimal lightingCostPerSession)
        {
            var court = new Court
            {
                Name = name,
                Status = CourtStatus.Available,
                LightingCostPerSession = lightingCostPerSession
            };

            await _context.Courts.AddAsync(court);
            await _context.SaveChangesAsync();

            return court;
        }
    }
}
