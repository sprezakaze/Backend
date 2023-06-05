namespace webapi.Entities
{
    public class Clothing
    {
        public int ClothingId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Category? Category { get; set; }
        public int Price { get; set; } 
        public string? ProductPicture { get; set; }
        public string? InStock { get; set; } // ?
    }
}
