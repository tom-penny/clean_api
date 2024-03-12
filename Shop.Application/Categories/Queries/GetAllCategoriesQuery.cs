namespace Shop.Application.Categories.Queries;

using Domain.Entities;

public record GetAllCategoriesQuery() : IRequest<Result<List<Category>>>;