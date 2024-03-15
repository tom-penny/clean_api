namespace Shop.API.Mappings;

using Models.Responses;
using Domain.Entities;
using Application.Common.Models;

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
    
    public static ProductsResponse ToResponse<T>(this PagedList<T> list) where T : Product
    {
        return new ProductsResponse
        {
            Page = list.Page,
            Size = list.Size,
            Count = list.Count,
            Products = list.Items.Select(p => p.ToResponse()).ToList(),
        };
    }
}