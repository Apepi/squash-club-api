using Microsoft.EntityFrameworkCore;
using SquashClubAPI.Data;
using SquashClubAPI.Models;

namespace SquashClubAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;  // This to update user balance

        public TransactionService(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Transaction> AddFunds(int userId, decimal amount)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) { return null; }

            var transaction = new Transaction
            {
                UserId = userId,
                Amount = amount,
                TransactionType = TransactionType.Credit,
                Description = "Added funds to account",
                DateTime = DateTime.UtcNow

            };

            user.AccountBalance += amount;
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<Transaction> ProcessPayment(int userId, decimal amount, string description)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.AccountBalance < amount)
                return null;

            var transaction = new Transaction
            {
                UserId = userId,
                Amount = amount,
                TransactionType = TransactionType.Debit,
                Description = description,
                DateTime = DateTime.UtcNow
            };

            user.AccountBalance -= amount;
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetUserTransactions(int userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.DateTime)
                .ToListAsync();
        }

        public async Task<Transaction> GetTransactionById(int transactionId)
        {
            return await _context.Transactions.FindAsync(transactionId);
        }
    }
}
