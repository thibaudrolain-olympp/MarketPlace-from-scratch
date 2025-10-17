namespace Marketplace.Domain.DataModels
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}