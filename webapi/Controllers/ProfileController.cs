using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Dtos;
using webapi.Entities;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly AccountService _accountService;
        public ProfileController(AuthService authService, AccountService accountService)
        {
            _authService = authService;
            _accountService = accountService;
        }
        [HttpGet]
        public async Task<ActionResult<ProfileDto>> Profile()
        {
            try
            {
                var sessId = HttpContext.Request.Cookies["sessid"]
                    ?? throw new Exception("Session is incorrect");
                var user = await _authService.GetUser(sessId);
                var profile = await _accountService.GetProfile(user);

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet, Route("cart")]
        public async Task<ActionResult<List<CartItem>>> GetCart()
        {
            try
            {
                var sessId = HttpContext.Request.Cookies["sessid"]
                    ?? throw new Exception("Session is incorrect");
                var user = await _authService.GetUser(sessId);
                var profile = await _accountService.GetProfile(user);

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
