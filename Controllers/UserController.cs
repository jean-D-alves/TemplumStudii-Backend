using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using templumStudii.dtos;
using templumStudii.Dtos.User;
using templumStudii.models;
using templumStudii.services;

namespace templumStudii.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserService userService, AuthServices authServices) : ControllerBase
    {
        private readonly UserService userService = userService;
        private readonly AuthServices authServices = authServices;

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId =
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier
                )?.Value;
            var name =
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.Name
                )?.Value;

            var email =
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.Email
                )?.Value;

            return Ok(new
            {
                UserId = userId,
                Name = name,
                Email = email
            });
        }
        [Authorize]
        [HttpDelete()]
        public async Task<ActionResult> DeleteAsync()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { Message = "Token inválido" });
            }

            var user = await userService.DeleteUserAsync(userId);
            return Ok(user);
        }


        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] User request)
        {
            try
            {
                User user = await userService.CreateAsync(request);
                string token = authServices.GenerateToken(user);

                UserResponse userResponse = new UserResponse
                {
                    Id = user.Id,
                    Name = user.name,
                    Email = user.email,
                    Studies = user.studies
                };

                return Ok(new { User = userResponse, Token = token });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message }); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                User user =
                    await userService.GetUserByEmailAsync(request);

                string token =
                    authServices.GenerateToken(user);

                UserResponse userResponse = new UserResponse
                {
                    Id = user.Id,
                    Name = user.name,
                    Email = user.email,
                    Studies = user.studies
                };

                return Ok(new
                {
                    User = userResponse,
                    Token = token
                });
            }
            catch (KeyNotFoundException)
            {
                return Unauthorized(new
                {
                    Message = "Email ou senha inválidos"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    $"Internal server error: {ex.Message}"
                );
            }
        }
    }
}
