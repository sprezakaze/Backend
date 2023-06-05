using Microsoft.AspNetCore.Mvc;
using webapi.Entities;
using Microsoft.EntityFrameworkCore;

namespace webapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClothingController : ControllerBase
    {
        private readonly ClothingContext _dbContext;
        public ClothingController(ClothingContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clothing>>> GetClothes()
            => Ok(await _dbContext.Clothings.Include(el => el.Category).ToListAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Clothing>> GetClothing(int id)
        {
            var clothing = await _dbContext.Clothings
                .Include(el => el.Category)
                .FirstOrDefaultAsync(el => el.ClothingId == id);

            if (clothing == null)
            {
                return NotFound();
            }
            return clothing;
        }
        
        [HttpPost]
        public async Task<ActionResult<Clothing>> PostClothing(Clothing clothing)
        {
            await _dbContext.Clothings.AddAsync(clothing);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClothes), new { id = clothing.ClothingId }, clothing);
        }

        [HttpPut]
        public async Task<IActionResult> PutClothing(int id, Clothing clothing)
        {
            if(id!=clothing.ClothingId)
            {
                return BadRequest();
            }
            _dbContext.Entry(clothing).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!ClothingAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }
       private bool ClothingAvailable(int id) 
        {
            return (_dbContext.Clothings?.Any(x => x.ClothingId == id)).GetValueOrDefault();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClothing(int id)
        {
            if(_dbContext.Clothings ==null)
            {
                return NotFound();
            }
            var clothing = await _dbContext.Clothings.FindAsync(id);
            if(clothing == null)
            {
                return NotFound();
            }
            _dbContext.Clothings.Remove(clothing);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
