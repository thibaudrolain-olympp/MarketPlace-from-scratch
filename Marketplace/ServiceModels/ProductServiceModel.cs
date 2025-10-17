namespace Marketplace.ServiceModels
{
    public class ProductServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public CategoryServiceModel Category { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<ProductImageServiceModel> Images { get; set; }

        public ProductServiceModel()
        { }

        public ProductServiceModel(int id, string name, string description, decimal price, int quantity, CategoryServiceModel category, string status, DateTime createdAt, DateTime updatedAt, List<ProductImageServiceModel> productImages)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            Category = category;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Images = productImages;
        }
    }
}