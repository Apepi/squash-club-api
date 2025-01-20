using SquashClubAPI.Models;

public interface ITransactionService
{
    Task<Transaction> AddFunds(int userId, decimal amount);  // Credit
    Task<Transaction> ProcessPayment(int userId, decimal amount, string description);  // Debit
    Task<IEnumerable<Transaction>> GetUserTransactions(int userId);  // Transaction history
    Task<Transaction> GetTransactionById(int transactionId);
}