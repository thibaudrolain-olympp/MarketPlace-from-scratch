namespace Marketplace.DataModels
{
    public class Order
    {
        public int Id { get; set; }

        public int BuyerId { get; set; }
        public User Buyer { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } // pending, paid, shipped, delivered

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}