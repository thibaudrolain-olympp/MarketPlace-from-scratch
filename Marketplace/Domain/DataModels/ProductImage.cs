namespace Marketplace.Domain.DataModels
{
    public class ProductImage
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public string ImageUrl { get; set; }

        public bool IsMain { get; set; }
    }
}