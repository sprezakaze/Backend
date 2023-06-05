using webapi.Entities;

namespace webapi.Dtos
{
    public class CategoryClothesDto
    {
        public Category Category { get; set; }
        public List<Clothing> Clothings { get; set; }
    }
}
