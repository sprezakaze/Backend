namespace webapi.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public DateTime Expire { get; set; }
    }
}
