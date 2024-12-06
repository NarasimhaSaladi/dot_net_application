using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginAuthApp.Services;

namespace LoginAuthApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _authService.RegisterAsync(
                request.Username, 
                request.Password, 
                request.Email);
            
            return Ok(new { UserId = user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var isValid = await _authService.LoginAsync(
                request.Username, 
                request.Password);
            
            return isValid 
                ? Ok(new { Message = "Login Successful" }) 
                : Unauthorized();
        }

        // Request models
        public class RegisterRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}