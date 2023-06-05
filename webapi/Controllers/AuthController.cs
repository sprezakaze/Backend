using Microsoft.AspNetCore.Mvc;
using webapi.Dtos;
using webapi.Entities;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly AuthService _authService;
        public AuthController(AuthService userService) 
        {
            _authService = userService; 
        }

        [HttpPost, Route("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginDto dto)
        {
            try
            {
                // Это деструктуризация
                // см. https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/deconstruct
                (var session, var user) = await _authService.Login(dto);
                HttpContext.Response.Cookies.Append(
                    "sessid",
                    session.Id.ToString(),
                    new CookieOptions
                    {
                        Expires = session.Expire,
                        HttpOnly = true
                    });
                return Ok(user);
            } 
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost, Route("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterDto dto)
        {
            try
            {
                (var session, var user) = await _authService.Register(dto);
                HttpContext.Response.Cookies.Append(
                    "sessid",
                    session.Id.ToString(),
                    new CookieOptions { 
                        Expires = session.Expire, 
                        HttpOnly = true 
                    });
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
