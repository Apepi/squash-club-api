using SquashClubAPI.Data;      
using SquashClubAPI.Models;    
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace SquashClubAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterUser(string username, string email, string password)       // Register User Method
        {
            var user = new User
            {
                Name = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                AccountBalance = 0
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> AuthenticateUser(string email, string password)     // Authenticate User Method
        {
            var user = await _context.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task<User> GetUserById(int id)     // Get User Method
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> UpdateUser(int id, User updatedUser) // Update User method
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            user.Name= updatedUser.Name;
            user.Email= updatedUser.Email;

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<decimal> GetUserBalance(int id)  // Get User Balance Method
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {  return 0; }

            return user.AccountBalance;

        }

        public async Task<User> GetUserByEmail(string email)  // Get User Email Method
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}

