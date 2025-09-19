using System.ComponentModel.DataAnnotations;

namespace Marketplace.DataModels
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public int SellerProfileId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Category Category { get; set; }

        public string Status { get; set; } // active, sold_out, inactive

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<ProductImage> Images { get; set; }
    }
}