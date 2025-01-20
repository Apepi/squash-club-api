using SquashClubAPI.Models;

namespace SquashClubAPI.Services
{
    public interface IUserService
    {
        Task<User> RegisterUser(string username, string email, string password); 
        Task<bool> AuthenticateUser(string email, string password);  
        Task<User> GetUserById(int id);  
        Task<bool> UpdateUser(int id, User updatedUser);  
        Task<decimal> GetUserBalance(int id);  

        Task<User> GetUserByEmail(string email);

    }
}