
using Logs.DTO;
using Logs.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("session")]
        public IActionResult GetSession()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
                return Unauthorized("No active session found.");

            // Get user info from claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;
            var nameClaim = User.FindFirst(ClaimTypes.Name)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Session is invalid.");

            var sessionInfo = new
            {
                UserId = userIdClaim,
                Email = emailClaim,
                Name = nameClaim
            };

            return Ok(sessionInfo);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var result = await _userServices.CreateUserAsync(dto);
            if (result.success)
            {
                return Ok(result.message);
            }
            return BadRequest(result.message);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var result = await _userServices.LoginUserAsync(dto);

            if (!result.success || result.user == null)
                return BadRequest(result.message);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, result.user.Userid.ToString()),
            new Claim(ClaimTypes.Email, result.user.Email),
            new Claim(ClaimTypes.Name, result.user.Firstname +" "+ result.user.Firstname)
        };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                });

            return Ok("Login successful");
        }


        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userServices.GetAllUsersAsync();
            if (result.success)
            {
                return Ok(result.users);
            }
            return BadRequest(result.message);
        }
    }
}
