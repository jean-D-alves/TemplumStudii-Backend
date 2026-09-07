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
        [HttpGet]
        public async Task<ActionResult<List<User>>> ReaderAsync()
        {
            try
            { 
                List<User> response = await userService.ReaderAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId =
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier
                )?.Value;

            var email =
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.Email
                )?.Value;

            return Ok(new
            {
                UserId = userId,
                Email = email
            });
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var user = await userService.DeleteUserAsync(id);
                if (user == null) return NotFound();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
        [HttpPost]
        public async Task<ActionResult<User>> CreateAsync([FromBody]User request)
        {
            try
            {
                User user = await userService.CreateAsync(request);
                if (user == null) return NotFound();
                UserResponse response = new UserResponse
                {
                    Id = user.Id,
                    Name = user.name,
                    Email = user.email,
                    Studies = user.studies
                };
                return Ok(response);
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
