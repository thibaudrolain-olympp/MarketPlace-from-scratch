namespace Marketplace.Application.ServiceModels
{
    public class CategoryServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }
        public ICollection<ProductServiceModel> Products { get; set; }
    }
}