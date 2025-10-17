namespace Marketplace.Application.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }
        public ICollection<ProductDto> Products { get; set; }
    }
}