using SquashClubAPI.Models;

namespace SquashClubAPI.Services
{
    public interface ICourtService
    {
        Task<IEnumerable<Court>> GetAllCourts();
        Task<Court> GetCourtById(int id);
        Task<bool> UpdateCourtStatus(int id, CourtStatus status);
        Task<bool> IsCourtAvailable(int id, DateTime startTime, DateTime endTime);
        Task<Court> CreateCourt(string name, decimal lightingCostPerSession);
    }
}
