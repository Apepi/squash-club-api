using SquashClubAPI.Models;

namespace SquashClubAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        bool ValidateToken(string token);
    }
}
