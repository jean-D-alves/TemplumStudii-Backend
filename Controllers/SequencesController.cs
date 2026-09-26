using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using templumStudii.services;
using TemplumStudii.services;

namespace TemplumStudii.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SequencesController : ControllerBase
    {

        private readonly SequenceServices _sequenceServices;
        private readonly ILogger<SequencesController> _logger;

        public SequencesController(SequenceServices sequenceServices, ILogger<SequencesController> logger)
        {
            _sequenceServices = sequenceServices;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateSequence()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var sequence = await _sequenceServices.AddAsync(userId);
                return Ok(sequence);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar a sequência do usuário");
                return StatusCode(500, new { message = "Erro interno ao criar a sequência." });
            }
        }
    }
}
