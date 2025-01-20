using Microsoft.AspNetCore.Mvc;
using SquashClubAPI.Models;
using SquashClubAPI.Services;

namespace SquashClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _userService.RegisterUser(request.Username, request.Email, request.Password);
            if (user == null)
                return BadRequest("Registration failed");

            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var isAuthenticated = await _userService.AuthenticateUser(request.Email, request.Password);
            if (!isAuthenticated)
                return Unauthorized();

            var user = await _userService.GetUserByEmail(request.Email);
            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}