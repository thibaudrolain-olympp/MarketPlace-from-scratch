namespace Marketplace.Application.ServiceModels
{
    public class CartItemServiceModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}