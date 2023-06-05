using Microsoft.EntityFrameworkCore;
using webapi.Dtos;
using webapi.Entities;

namespace webapi.Services
{
    public class OrderService
    {
        private readonly ClothingContext _context;
        public OrderService(ClothingContext context) 
        { 
            _context = context; 
        }

        public async Task<(Order, List<CartItem>)> FormingOrder(User user, List<CreateCartItemDto> itemDtos, DateTime time)
        {
            if (time < DateTime.UtcNow) 
            {
                throw new InvalidDataException("time of order can't be lt than now");
            }
            var clothingIds = itemDtos.Select(el => el.ClothingId).ToList();
            var clothings = await _context.Clothings
                .Where(el => clothingIds.Contains(el.ClothingId))
                .ToListAsync();

            var order = new Order
            {
                Created = DateTime.UtcNow,
                Status = OrderStatus.InProgress,
                Id = new Guid(),
                User = user ?? throw new ArgumentException("User is not found"),
                Time = time,
            };

            var cartItems = itemDtos.Select(el => new CartItem { 
                Clothing = clothings.First(cloth => el.ClothingId == cloth.ClothingId),
                Count = el.Count,
                Id = new Guid(),
                Order = order
            }).ToList();
            await _context.Orders.AddAsync(order);
            await _context.AddRangeAsync(cartItems);
            await _context.SaveChangesAsync();

            var selectedOrder = await _context.Orders
                .FirstOrDefaultAsync(el => el.Id == order.Id)
                ?? throw new Exception("db error");
            var selectedItems = await _context.CartItems
                .Where(el => el.Order.Id == selectedOrder.Id)
                .Include(el => el.Clothing)
                .Include(el => el.Order)
                .ToListAsync();

            return (selectedOrder, selectedItems);
        }

        public async Task<List<CartItem>> GetOrders(User user)
        {
            var orders = await _context.CartItems
                .Where(el => el.Order.User.Id == user.Id)
                .Include(el => el.Clothing)
                .Include(el => el.Order)
                .ToListAsync();
            
            return orders;
        }
    }
}
