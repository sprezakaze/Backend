namespace webapi.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime Time { get; set; }
        public DateTime Created { get; set; }
    }

    public enum OrderStatus
    { 
        Cart = 1,
        InProgress,
        Done,
    }
}
