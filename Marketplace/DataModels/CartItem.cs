namespace Marketplace.DataModels
{
    public class CartItem
    {
        public int Id { get; set; }
        public Cart Cart { get; set; } = null!;

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}