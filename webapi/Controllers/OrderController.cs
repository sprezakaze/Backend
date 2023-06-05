using Microsoft.AspNetCore.Mvc;
using webapi.Dtos;
using webapi.Services;

namespace webapi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private OrderService _orderService;
        private AuthService _authService;
        private MailService _mailService;

        public OrderController(OrderService orderService, AuthService authService, MailService mailService) 
        { 
            _orderService = orderService;
            _authService = authService;
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll() 
        {
            try
            {
                var sessid = HttpContext.Request.Cookies["sessid"] ?? throw new Exception("Invalid session");
                var user = await _authService.GetUser(sessid) ?? throw new Exception("User is not found");
                var orders = await _orderService.GetOrders(user);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> CreateOrder(CreateOrderDto dto) 
        { 
            try
            {
                var sessid = HttpContext.Request.Cookies["sessid"] ?? throw new Exception("Invalid session");
                var user = await _authService.GetUser(sessid) ?? throw new Exception("User is not found");
                (var newOrder, var cartItems) = await _orderService.FormingOrder(user, dto.cart, DateTime.UtcNow.AddDays(3));

                await _mailService.SendOrder(newOrder, cartItems, dto.vk);

                return Ok(newOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
