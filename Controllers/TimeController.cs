using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using templumStudii.services;
using TemplumStudii.DTOs.Time;

namespace TemplumStudii.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        private readonly TimeService _timeService;
        private readonly ILogger<TimeController> _logger;

        public TimeController(TimeService timeService, ILogger<TimeController> logger)
        {
            _timeService = timeService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrent()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var time = await _timeService.FindByUserIdAsync(userId);
                return Ok(time);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar o tempo do usuário");
                return StatusCode(500, new { message = "Erro interno ao buscar o registro de tempo." });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Define(DefineRequest dto)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var time = await _timeService.DefineAsync(dto.definedTime, userId);
                return Ok(time);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao definir o tempo do usuário");
                return StatusCode(500, new { message = "Erro interno ao definir o tempo." });
            }
        }

        [HttpPost("start")]
        [Authorize]
        public async Task<IActionResult> Start()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var time = await _timeService.StartAsync(userId);
                return Ok(time);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {

                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao iniciar o cronômetro ");
                return StatusCode(500, new { message = "Erro interno ao iniciar o cronômetro." });
            }
        }

        [HttpPost("stop")]
        [Authorize]
        public async Task<IActionResult> Stop()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _timeService.StopAsync(userId);

                if (result is null)
                    return NotFound();

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao parar o cronômetro ");
                return StatusCode(500, new { message = "Erro interno ao parar o cronômetro." });
            }
        }

        [HttpGet("validate")]
        [Authorize]
        public async Task<IActionResult> Validate()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _timeService.ValidateTimeAsync(userId);
                return Ok(new { message = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar o tempo do usuário");
                return StatusCode(500, new { message = "Erro interno ao validar o tempo." });
            }
        }
    }
}