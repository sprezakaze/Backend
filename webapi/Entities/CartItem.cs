namespace webapi.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Order Order { get; set; }
        public Clothing Clothing { get; set; }
        public int Count { get; set; }
    }
}
