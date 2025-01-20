using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquashClubAPI.Models;
using SquashClubAPI.Services;

namespace SquashClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourtController : ControllerBase
    {
        private readonly ICourtService _courtService;

        public CourtController(ICourtService courtService)
        {
            _courtService = courtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourts()
        {
            var courts = await _courtService.GetAllCourts();
            return Ok(courts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourt(int id)
        {
            var court = await _courtService.GetCourtById(id);
            if (court == null)
                return NotFound();
            return Ok(court);
        }

        [Authorize] // Only authenticated users can check availability
        [HttpGet("{id}/availability")]
        public async Task<IActionResult> CheckAvailability(int id, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            var isAvailable = await _courtService.IsCourtAvailable(id, startTime, endTime);
            return Ok(new { IsAvailable = isAvailable });
        }

        [Authorize] // Only authenticated users can update status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] CourtStatus status)
        {
            var success = await _courtService.UpdateCourtStatus(id, status);
            if (!success)
                return NotFound();
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCourt([FromBody] CreateCourtRequest request)
        {
            var court = await _courtService.CreateCourt(request.Name, request.LightingCostPerSession);
            return Ok(court);
        }

        public class CreateCourtRequest
        {
            public string Name { get; set; }
            public decimal LightingCostPerSession { get; set; }
        }
    }
}