namespace Marketplace.Application.Dto
{
    public class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<CartItemDto> Items { get; set; } = new();
    }
}