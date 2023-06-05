namespace webapi.Dtos
{
    public class CreateOrderDto
    {
        public List<CreateCartItemDto> cart { get; set; }
        public string vk { get; set; }
    }
}
