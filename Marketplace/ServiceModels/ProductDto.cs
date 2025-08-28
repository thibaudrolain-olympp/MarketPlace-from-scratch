namespace Marketplace.ServiceModels
{
    public record ProductDto(int Id, string Name, string Description, decimal Price, int QuantityId, int CategoryId, string Status, DateTime CreatedAt, DateTime UpdatedAt);
}
