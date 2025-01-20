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
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("add-funds")]
        public async Task<IActionResult> AddFunds([FromBody] AddFundsRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var transaction = await _transactionService.AddFunds(userId, request.Amount);

            if (transaction == null)
                return BadRequest("Failed to add funds");

            return Ok(transaction);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetTransactionHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var transactions = await _transactionService.GetUserTransactions(userId);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionById(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }
    }

    public class AddFundsRequest
    {
        public decimal Amount { get; set; }
    }
}