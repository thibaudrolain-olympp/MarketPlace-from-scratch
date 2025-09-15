namespace Marketplace.DataModels
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}