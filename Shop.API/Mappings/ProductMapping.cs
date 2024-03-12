namespace Shop.API.Mappings;

using Domain.Entities;
using Models.Responses;

public static class ProductMapping
{
    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse
        {
            Id = product.Id.Value.ToString(),
            Name = product.Name,
            Stock = product.Stock,
            Price = product.Price,
            Categories = product.Categories.Select(c => c.ToResponse()).ToList()
        };
    }
}