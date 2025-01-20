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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserById(userId);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.AccountBalance
            });
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var balance = await _userService.GetUserBalance(userId);
            return Ok(new { Balance = balance });
        }
    }
}