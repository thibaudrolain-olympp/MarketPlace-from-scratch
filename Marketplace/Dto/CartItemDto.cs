namespace Marketplace.Dto
{
    public class CartItemDto
    {
        public ProductDto ProductDto { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } = decimal.Zero;
    }
}