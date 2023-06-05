using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Entities;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ClothesService _clothesService;
        public CategoryController(ClothesService clothesService)
        {
            _clothesService = clothesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAll()
        {
            return Ok(await _clothesService.GetCategories());
        }
        [HttpGet, Route("{id}")]
        public async Task<ActionResult<List<Category>>> GetClothesByCategory(string id)
        {
            return Ok(await _clothesService.GetClothesByCategory(id));
        }
    }
}
