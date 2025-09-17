namespace Marketplace.Dto
{
    public class AddCartItemRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } = decimal.Zero;
    }
}