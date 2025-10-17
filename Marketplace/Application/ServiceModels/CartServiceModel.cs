namespace Marketplace.Application.ServiceModels
{
    public class CartServiceModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<CartItemServiceModel> Items { get; set; } = new();
    }
}