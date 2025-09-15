namespace Marketplace.ServiceModels
{
    public class ProductServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityId { get; set; }
        public int CategoryId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ProductServiceModel()
        { }

        public ProductServiceModel(int id, string name, string description, decimal price, int quantityId, int categoryId, string status, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            QuantityId = quantityId;
            CategoryId = categoryId;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}