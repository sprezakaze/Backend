using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Dtos;
using webapi.Entities;

namespace webapi.Services
{
    public class ClothesService
    {
        private readonly ClothingContext _context;
        public ClothesService(ClothingContext context) 
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategories() 
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }
        public async Task<CategoryClothesDto> GetClothesByCategory(string id)
        {
            var categoryUuid = new Guid(id);
            var category = await _context.Categories.FirstOrDefaultAsync(el => el.Id == categoryUuid) 
                ?? throw new Exception("Category not found");

            var clothes = await _context.Clothings
                .Where(el => el.Category.Id == category.Id)
                .ToListAsync();

            var dto = new CategoryClothesDto { 
                Category = category, 
                Clothings = clothes 
            };

            return dto;
        }
    }
}
