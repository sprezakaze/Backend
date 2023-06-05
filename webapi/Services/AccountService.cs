using Microsoft.EntityFrameworkCore;
using webapi.Dtos;
using webapi.Entities;

namespace webapi.Services
{
    public class AccountService
    {
        private readonly ClothingContext _context;
        public AccountService(ClothingContext context) 
        { 
            _context = context; 
        }

        public async Task<ProfileDto> GetProfile(User user)
        {
            var cartItems = await _context.CartItems
                .Where(cart => cart.Order.User.Id == user.Id)
                .Include(el => el.Clothing)
                .ToListAsync();

            var profile = new ProfileDto { 
                User = user, 
                Cart = cartItems 
            };

            return profile;
        }
        public async Task<List<CartItem>> GetCart(User user)
        {
            var cartItems = await _context.CartItems
                .Where(cart => cart.Order.User.Id == user.Id)
                .Include(el => el.Clothing)
                .ToListAsync();

            return cartItems;
        }
    }
}
