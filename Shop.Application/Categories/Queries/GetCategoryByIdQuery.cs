namespace Shop.Application.Categories.Queries;

using Domain.Entities;

public record GetCategoryByIdQuery(Guid Id) : IRequest<Result<Category>>;